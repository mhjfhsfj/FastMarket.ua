using System.ComponentModel.DataAnnotations;
using FastMarketBackEnd.models;

namespace FastMarketBackEnd.DTOs;

public class ProductDTO
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock_quantity { get; set; }
    public int? CategoryId { get; set; }
    public int? SellerId { get; set; }
    public List<IFormFile>? Images { get; set; }
    public List<PictureProductDTO>? Pictures { get; set; }
} 