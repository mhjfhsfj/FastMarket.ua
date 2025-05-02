using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FastMarketBackEnd.models;

public class Characteristics
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public int NameCharacteristicsId { get; set; }
    public NameCharacteristics? NameCharacteristics { get; set; }
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
}