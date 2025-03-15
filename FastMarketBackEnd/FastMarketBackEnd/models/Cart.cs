using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Cart
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [ForeignKey("UserID")]
    // [ForeignKey("Id")]
    public User User { get; set; }
    
    [ForeignKey("ProductID")]
    // [ForeignKey("Id")]
    public Product Product { get; set; }
    
    public int Quantity { get; set; }
}