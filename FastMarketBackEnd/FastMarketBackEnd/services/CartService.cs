using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;
using FastMarketBackEnd.Utility;
using Microsoft.EntityFrameworkCore;

namespace FastMarketBackEnd.services;

public class CartService
{
    private readonly ApplicationContext _context;
    private readonly ILogger<CartService> _logger;
    
    public CartService(ApplicationContext context, ILogger<CartService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Cart> AddToCartAsync(Cart cart)
    {
        _logger.LogInformation("Add to cart start");
        var cartTemp = cart;
        var product = await _context.Products.FindAsync(cart.ProductId);
        if (product == null)
        {
            _logger.LogWarning("Product not found");
            throw new NotFoundException("Product not found",404);
        }
        if (cart.Quantity > product.Stock_quantity)
        {
            _logger.LogWarning("Not enough product");
            throw new NotFoundException("Not enough product",404);
        }
        cartTemp.Price = product.Price*cart.Quantity;;
        _context.Carts.Add(cartTemp);
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error added cart");
        //     throw new Exception("Error added cart");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Add to cart successfully");
        return await GetCartByIdAsync(cartTemp.Id);
    }

    public async Task<List<Cart>> GetCartsByUserIdAsync(int userId)
    {
        _logger.LogInformation("Get carts by user id start");
        var carts = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
        if (carts == null)
        {
            _logger.LogWarning("Carts not found");
            throw new NotFoundException("Carts not found",404);
        }
        _logger.LogInformation("Get carts by user id successfully");
        return carts;
    }

    public async Task<List<Cart>> GetCartsAsync()
    {
        _logger.LogInformation("Get carts start");
        var carts = await _context.Carts.ToListAsync();
        if (carts == null)
        {
            _logger.LogWarning("Carts not found");
            throw new NotFoundException("Carts not found",404);
        }
        _logger.LogInformation("Get carts successfully");
        return carts;
    }
    
    public async Task<Cart> GetCartByIdAsync(int id)
    {
        _logger.LogInformation("Get cart by id start");
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found");
            throw new NotFoundException("Cart not found",404);
        }
        _logger.LogInformation("Get cart by id successfully");
        return cart;
    }

    public async Task ChangeCartAsync(int id, Cart cart)
    {
        _logger.LogInformation("Change cart start");
        var cartTemp = await _context.Carts.FindAsync(id);
        if (id != cart.Id)
        {
            _logger.LogWarning("Id not match");
            throw new NotFoundException("Id not match",404);
        }

        if (cartTemp != null)
        {
            cartTemp.Quantity = cart.Quantity;
            _context.Entry(cartTemp).State = EntityState.Modified;
        }

        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error change cart");
        //     throw new Exception("Error change cart");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Change cart successfully");
    }
    
    public async Task DeleteCartAsync(int id)
    {
        _logger.LogInformation("Delete cart start");
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found");
            throw new NotFoundException("Cart not found",404);
        }
        _context.Carts.Remove(cart);
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error delete cart");
        //     throw new Exception("Error delete cart");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Delete cart successfully");
    }

    public async Task DeleteCartsByUserIdAsync(int userId)
    {
        _logger.LogInformation("Delete carts by user id start");
        var carts = await GetCartsByUserIdAsync(userId);

        if (carts == null)
        {
            
            _logger.LogWarning("Carts not found");
            throw new NotFoundException("Carts not found",404);
        }
        _context.Carts.RemoveRange(carts);
        
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error delete carts");
        //     throw new Exception("Error delete carts");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Delete carts by user id successfully");
    }
    
}