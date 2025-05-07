namespace FastMarketBackEnd.DTOs;

public class LiqPayCallback
{
    public string order_id { get; set; }
    public string status { get; set; }

    public string amount { get; set; }
    public string currency { get; set; }
}