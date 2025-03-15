using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Review
{
    [Key]
    [Required]
    public int Id { get; set; }
    [ForeignKey("UserID")]
    public User User { get; set; } = null!;
    [ForeignKey("ProductID")]
    public Product Product { get; set; } = null!;

    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   
}