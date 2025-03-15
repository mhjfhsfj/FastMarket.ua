using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarketBackEnd.models;

public class Seller
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [ForeignKey("UserID")]
    public User User { get; set; }
    
    
}