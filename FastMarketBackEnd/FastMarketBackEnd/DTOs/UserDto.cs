using FastMarketBackEnd.models;

namespace FastMarketBackEnd.DTOs;

public class UserDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; } = null!;
    public string? SecondName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? email { get; set; }
    public string phone { get; set; } 
    public Role Role { get; set; } = Role.user;
    public SellerDTO? Seller { get; set; }
}