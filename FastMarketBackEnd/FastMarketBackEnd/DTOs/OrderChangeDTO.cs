using FastMarketBackEnd.models;

namespace FastMarketBackEnd.DTOs;

public class OrderChangeDTO
{
    public int                     Id { get; set; }
    
    public string                  DeliveryAddress { get; set; } = string.Empty;
    public string                  Phone { get; set; } = string.Empty;
    public decimal                 TotalPrice { get; set; } = 0;

    public List<OrderDetails>     OrderDetails { get; set; }
    public StatusOrder?           StatusOsrder { get; set; } = models.StatusOrder.Pending;
}