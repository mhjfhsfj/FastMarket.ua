namespace FastMarketBackEnd.DTOs;

public class SmsDTO
{
    public List<string> phone { get; set; }
    public string message { get; set; }
    public string src_addr { get; set; } = "AUTO";
}