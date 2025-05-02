using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Rating
{
    [Key]
    [Required]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Range(1, 5, ErrorMessage = "Оцінка має бути від 1 до 5.")]
    public int Score { get; set; }

    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}