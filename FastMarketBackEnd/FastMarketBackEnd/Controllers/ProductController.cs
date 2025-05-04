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
    
    //--------------------------------------------------------------------------------------------------------------

    [HttpGet(nameof(GetModeratedProducts))]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetModeratedProducts()
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetModeratedProducts();
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------

    [HttpGet(nameof(GetNotModeratedProducts))]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetNotModeratedProducts()
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetNotModeratedProducts();
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------

    [HttpGet(nameof(GetAllProducts))]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetAllProducts();
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------
    
    [HttpGet(nameof(GetProductById))]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetProductById(id);
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------

    [HttpGet(nameof(GetProductByCategoryId))]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByCategoryId(int id)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetProductByCategoryId(id);
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------

    [HttpGet(nameof(GetProductBySellerId))]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductBySellerId(int id)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.GetProductBySellerId(id);
            return product;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------
    
    [HttpPost(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct( List<IFormFile> images, ProductDTO productDto)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            var product = await _catalogServices.CreateProduct(images, productDto);
            return Ok(product);

        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut(nameof(ChangeProduct) + "/{id}")]
    public async Task<IActionResult> ChangeProduct(int id, ProductDTO productDto)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
           await _catalogServices.ChangeProductByCategoryId(id, productDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new { message = e.Message });
        }
        
        return NoContent();
    }
    
    [HttpDelete(nameof(DeleteProduct)+"/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            _catalogServices.HttpContext = HttpContext;
            await _catalogServices.DeleteProductById(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new { message = e.Message });
        }
        return NoContent();
    }
    
    
} 