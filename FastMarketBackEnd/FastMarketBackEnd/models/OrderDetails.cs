using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class OrderDetails
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [ForeignKey("OrderID")]
    // [ForeignKey("Id")]
    public Order Order { get; set; }
    
    [ForeignKey("ProducrID")]
    // [ForeignKey("Id")]
    public Product Product { get; set; }
    
    public int Quantity { get; set; }
    public decimal Price_at_purchase { get; set; }
    
}