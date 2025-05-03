namespace FastMarketBackEnd.DTOs;

public class SellerDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public UserDto? User { get; set; }
}