using System.ComponentModel.DataAnnotations;

namespace FastMarketBackEnd.models;

public class StatusModeration
{
    [Key]
    [Required]
    public int Id { get; set; }
    public String Name { get; set; }
}