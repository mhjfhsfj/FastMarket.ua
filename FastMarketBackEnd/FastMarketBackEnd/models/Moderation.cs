using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Moderation
{
    [Key]
    [Required]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int UserID { get; set; }
    public User? User { get; set; }
    public StatusModeration StatusModeration { get; set; }
    public DateTime Review_date { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum StatusModeration
{
    moderated,
    notModerated,
    inProgress,
    refusal
}