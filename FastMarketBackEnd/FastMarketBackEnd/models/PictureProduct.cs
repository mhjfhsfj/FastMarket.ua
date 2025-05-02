using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FastMarketBackEnd.models;

public class PictureProduct
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    public string Link  { get; set; }
    public bool PreviewPicture { get; set; } = false;
    
    public int ProductId { get; set; }
    
    [JsonIgnore]
    public Product Product { get; set; }
}