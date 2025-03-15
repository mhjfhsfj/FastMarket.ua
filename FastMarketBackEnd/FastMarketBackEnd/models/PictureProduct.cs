using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class PictureProduct
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    public bool PreviewPicture { get; set; }
    
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
}