using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Favorite
{
    [Key]
    [Required]
    public int Id { get; set; }
    [ForeignKey("UserID")]
    public User User { get; set; }
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
}