namespace FastMarketBackEnd.models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public StatusOsrder? StatusOsrder { get; set; }
}