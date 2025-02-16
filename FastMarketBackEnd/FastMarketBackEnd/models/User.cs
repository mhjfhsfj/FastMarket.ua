namespace FastMarketBackEnd.models;

public class User
{
    public int Id { get; set; }
    public string fio { get; set; }
    public string email { get; set; }
    public string password_hash { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}