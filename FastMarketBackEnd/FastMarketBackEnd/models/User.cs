using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class User
{
    
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? Name { get; set; } = null!;
    public string? SecondName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? email { get; set; }
    public string? password_hash { get; set; }
    [Required]
    [Phone]
    public string phone { get; set; } 
    
    public Role Role { get; set; } = Role.user;
    
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool? IsActive { get; set; } = true;
    
    
    public Seller? Seller { get; set; }
}

public enum Role
{
    admin,
    user,
    moderator
}
