using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.services;
using Microsoft.AspNetCore.Mvc;

namespace FastMarketBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly CatalogServices _catalogServices;
    private readonly ILogger<ProductController> _logger;

    public ProductController(CatalogServices catalogServices, ILogger<ProductController> logger)
    {
        _catalogServices = catalogServices;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct()
    {
        try
        {
            var product = await _catalogServices.GetProducts();
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct( List<IFormFile> images, ProductDTO productDto)
    {
        try
        {
            var product = await _catalogServices.CreateProduct(images, productDto);
            return Ok(product);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
} 