namespace FastMarketBackEnd.models;



public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public String? Description { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Stock_quantity { get; set; } = 0;
    
    public int IdCategory { get; set; }
    public Сategory? Category { get; set; }
    public int IdUser { get; set; }
    public User? User { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}