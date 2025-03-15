using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Order
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [ForeignKey("UserID")]
    // [ForeignKey("Id")]
    public User User { get; set; }
    
    public decimal TotalPrice { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    
    
    public StatusOsrder? StatusOsrder { get; set; }
}