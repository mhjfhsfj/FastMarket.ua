using System.Text;
using System.Text.Json;
using FastMarketBackEnd.DTOs;

namespace FastMarketBackEnd.Utility;

public class NovaPostService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NovaPostService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _apiKey = config["SettingApp:NovaPostApi"];
    }

    private async Task<JsonElement> SendRequestAsync(string model, string method, object? properties = null)
    {
        var request = new
        {
            apiKey = _apiKey,
            modelName = model,
            calledMethod = method,
            methodProperties = properties ?? new { }
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.novaposhta.ua/v2.0/json/", content);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("data");
    }

    public async Task<List<NpRegion>> GetRegionsAsync()
    {
        var data = await SendRequestAsync("Address", "getAreas");

        return data.EnumerateArray()
            .Select(x => new NpRegion
            {
                Ref = x.GetProperty("Ref").GetString()!,
                Description = x.GetProperty("Description").GetString()!
            })
            .ToList();
    }

    public async Task<List<NpCity>> GetCitiesByRegionAsync(string regionRef)
    {
        var data = await SendRequestAsync("Address", "getCities", new { AreaRef = regionRef });

        return data.EnumerateArray()
            .Select(x => new NpCity
            {
                Ref = x.GetProperty("Ref").GetString()!,
                Description = x.GetProperty("Description").GetString()!,
                Address = x.TryGetProperty("DeliveryCity", out var addr) ? addr.GetString() : null
            })
            .ToList();
    }

    public async Task<List<NpWarehouse>> GetWarehousesAsync(string cityRef, string typeOfWarehouse = "All")
    {
        var props = new Dictionary<string, object> { { "CityRef", cityRef } };

        if (typeOfWarehouse != "All")
            props["TypeOfWarehouseRef"] = typeOfWarehouse;

        var data = await SendRequestAsync("Address", "getWarehouses", props);

        return data.EnumerateArray()
            .Select(x => new NpWarehouse
            {
                Ref = x.GetProperty("Ref").GetString()!,
                Description = x.GetProperty("Description").GetString()!,
                Address = x.GetProperty("ShortAddress").GetString()!
            })
            .ToList();
    }
}