using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Moderation
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [ForeignKey("ProductID")]
    // [ForeignKey("Id")]
    public Product Product { get; set; }
    
    [ForeignKey("UserID")]
    // [ForeignKey("Id")]
    public User User { get; set; }
    
    [ForeignKey("StatusModerationID")]
    // [ForeignKey("Id")]
    public StatusModeration StatusModeration { get; set; }
    public DateTime Review_date { get; set; } = DateTime.UtcNow;
}