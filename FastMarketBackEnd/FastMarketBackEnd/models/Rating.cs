using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Rating
{
    [Key]
    [Required]
    public int Id { get; set; }
    [ForeignKey("UserID")]
    public User User { get; set; }
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public int Score { get; set; } // Наприклад, оцінка від 1 до 5
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}