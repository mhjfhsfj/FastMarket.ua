namespace FastMarketBackEnd.models;

public class Moderation
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int StatusModerationId { get; set; }
    public StatusModeration? StatusModeration { get; set; }
    public DateTime Review_date { get; set; } = DateTime.Now;
}