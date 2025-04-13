namespace FastMarketBackEnd.DTOs;

public class CategoryDTO
{
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public PrimaryСategoryDTO? PrimaryCategoryDTO { get; set; }
}