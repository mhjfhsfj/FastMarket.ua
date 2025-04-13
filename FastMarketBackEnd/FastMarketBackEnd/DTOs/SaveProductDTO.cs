namespace FastMarketBackEnd.DTOs;

public class SaveProductDTO
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public String? Description { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Stock_quantity { get; set; } = 0;
    
    public CategoryDTO? Category { get; set; }
}