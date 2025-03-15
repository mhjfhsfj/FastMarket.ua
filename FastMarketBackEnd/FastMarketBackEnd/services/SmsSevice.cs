using System.Net.Http.Headers;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using Microsoft.EntityFrameworkCore;

namespace FastMarketBackEnd.services;

public class SmsSevice
{
    private readonly ApplicationContext _db;
    private readonly ILogger<TokensService> _logger;

    public SmsSevice(ApplicationContext db, ILogger<TokensService> logger)
    {
        this._db = db;
        this._logger = logger;
    }

    public async Task<HttpContent> SendSmsCodeAsync(AuthPhoneDTO authPhoneDto)
    {
        var httpClient = new HttpClient();
        var listPhone = new List<string>();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "E1w0O6eboxV-1CB");
        listPhone.Add(authPhoneDto.PhoneNumber);

        string code = GenaratorSmsCode();
        
        // отправляемый объект 
        SmsDTO sms = new SmsDTO { phone = listPhone, message = $"CODE: {code}" };
        
        // создаем JsonContent
        JsonContent content = JsonContent.Create(sms);
        Console.WriteLine(content.ReadAsStringAsync().Result);
        // отправляем запрос
        using var response = await httpClient.PostAsync("https://im.smsclub.mobi/sms/send", content);
        
        _logger.LogInformation(response.Content.ReadAsStringAsync().Result);
        await this._db.AddAsync(new UserPhoneCode
            { PhoneCode = code, PhoneNumber = authPhoneDto.PhoneNumber });
        await this._db.SaveChangesAsync();
        return response.Content;
    }

    public async Task<bool> ValidateCodeAsync(UserPhoneCodeDTO userPhoneCodeDto)
    {
        
        var user = await this._db.UserPhoneCodes
            .Where(u=>u.PhoneCode == userPhoneCodeDto.PhoneCode)
            .FirstOrDefaultAsync(u => u.PhoneNumber == userPhoneCodeDto.PhoneNumber);
        if (user is null)
        {
            var phone  = await this._db.UserPhoneCodes.FirstOrDefaultAsync(u => u.PhoneNumber == userPhoneCodeDto.PhoneNumber);
            this._db.UserPhoneCodes.Remove(phone);
            this._logger.LogError("SMS Code not true!");
            return false;
        }
        
        this._db.UserPhoneCodes.Remove(user);
        return true;
    }
    
    
    private string GenaratorSmsCode()
    {
        Random random = new Random();
            
        string code = random.Next(1000, 10000).ToString();
        return code;
    }
}