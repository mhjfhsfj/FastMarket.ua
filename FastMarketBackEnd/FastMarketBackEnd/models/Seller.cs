using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Seller
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool? IsActive { get; set; } = true;
}