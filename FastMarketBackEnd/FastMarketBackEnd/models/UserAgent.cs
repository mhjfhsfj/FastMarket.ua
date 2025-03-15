using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class UserAgent
{
    // [Key]
    [Required]
    public int Id { get; set; }

    [ForeignKey("TokenId")]
    public Token Token { get; set; }

    [Required]
    public string OS { get; set; }

    [Required]
    public string Browser { get; set; }
}