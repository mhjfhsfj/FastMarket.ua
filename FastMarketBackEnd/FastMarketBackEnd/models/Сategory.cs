namespace FastMarketBackEnd.models;

public class Сategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    
    public int IdPrimaryCategory { get; set; }
    public PrimaryСategory? PrimaryCategory { get; set; }
}