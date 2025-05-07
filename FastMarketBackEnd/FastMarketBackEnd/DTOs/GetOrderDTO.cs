using FastMarketBackEnd.models;

namespace FastMarketBackEnd.DTOs;

public class GetOrderDTO
{
    public int                     Id { get; set; }
    public User?                   User { get; set; }
    public Seller?                 Seller { get; set; }
    
    public string                  DeliveryAddress { get; set; } = string.Empty;
    public string                  Phone { get; set; } = string.Empty;
    public decimal                 TotalPrice { get; set; } = 0;
    public List<OrderDetails>     OrderDetails { get; set; }
    public StatusOrder?           StatusOsrder { get; set; } = models.StatusOrder.Pending;
    public PaymentMethod?         PaymentMethod { get; set; } = models.PaymentMethod.Card;
    public DeliveryMethod?        DeliveryMethod { get; set; } = models.DeliveryMethod.Postal;
    public PaymentStatus?         PaymentStatus { get; set; } = models.PaymentStatus.Pending;
}