using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Order
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public decimal TotalPrice { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


    public StatusOsrder? StatusOsrder { get; set; } = models.StatusOsrder.inProcessing;
}

public enum StatusOsrder
{
    inProcessing, 
    inDelivery, 
    received, 
    canceled
}