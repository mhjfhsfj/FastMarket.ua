using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Token
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    // [ForeignKey("Id")]
    public User User { get; set; }

    public UserAgent UserAgent { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime Expired { get; set; }

    [Required]
    public int LifeTime { get; set; } = 2;

    public Token()
    {
        this.Expired = this.Created.AddMinutes(this.LifeTime);
    }
}