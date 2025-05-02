using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FastMarketBackEnd.models;

public class Favorite
{
    [Key]
    [Required]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
}