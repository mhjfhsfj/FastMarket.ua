namespace FastMarketBackEnd.DTOs;

public class NpCity
{
    public string Ref { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Address { get; set; } // Інколи може бути null
}