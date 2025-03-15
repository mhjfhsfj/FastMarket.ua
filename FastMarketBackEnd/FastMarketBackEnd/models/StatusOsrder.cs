using System.ComponentModel.DataAnnotations;

namespace FastMarketBackEnd.models;

public class StatusOsrder
{
    [Key]
    [Required]
    public int Id { get; set; }
    public String name { get; set; }
}