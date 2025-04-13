namespace FastMarketBackEnd.DTOs;

public class PictureProductDTO
{
    public int? Id { get; set; }
    public string? FileName { get; set; }
    public string? Path { get; set; }
    public bool PreviewPicture { get; set; } = false;
    public int ProductID { get; set; }
}