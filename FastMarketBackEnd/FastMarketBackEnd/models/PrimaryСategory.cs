using System.ComponentModel.DataAnnotations;

namespace FastMarketBackEnd.models;

public class PrimaryСategory
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    public List<Сategory>? Categories { get; set; }
    
}

