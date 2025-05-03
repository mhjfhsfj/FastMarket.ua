using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;



public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public String? Description { get; set; }
    public decimal? Price { get; set; } = 0;
    public decimal AverageScore { get; set; } = 0;
    public int? Stock_quantity { get; set; } = 0;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int SellerId { get; set; }
    public Seller? Seller { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    public StatusModeration StatusModeration { get; set; } = StatusModeration.notModerated;
    public List<PictureProduct>? Pictures { get; set; }
    public List<Characteristics>? Characteristics { get; set; }
    public List<Favorite>? Favorites { get; set; }
    public List<Rating>? Ratings { get; set; }
    public List<Review>? Reviews { get; set; }
}