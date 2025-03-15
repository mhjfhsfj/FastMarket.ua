using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Сategory
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    [ForeignKey("PrimaryCategoryID")]
    // [ForeignKey("Id")]
    public PrimaryСategory? PrimaryCategory { get; set; }
}