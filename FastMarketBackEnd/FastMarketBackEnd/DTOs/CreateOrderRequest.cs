using FastMarketBackEnd.models;

namespace FastMarketBackEnd.DTOs;

public class CreateOrderRequest
{
    public int BuyerId { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    
    public PaymentMethod?         PaymentMethod { get; set; } = models.PaymentMethod.Card;
    public DeliveryMethod?        DeliveryMethod { get; set; } = models.DeliveryMethod.Postal;
    

    public List<OrderProductDto> Products { get; set; } = new();
}