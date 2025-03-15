using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;



public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public String? Description { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Stock_quantity { get; set; } = 0;
    [ForeignKey("CategoryID")]
    public Сategory? Category { get; set; }
    
    [ForeignKey("SellerID")]
    public Seller Seller { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    public List<PictureProduct>? Pictures { get; set; }
    public List<Favorite>? Favorites { get; set; }
    public List<Rating>? Ratings { get; set; }
    public List<Review>? Reviews { get; set; }
}