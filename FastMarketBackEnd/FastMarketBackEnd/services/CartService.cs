using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;
using FastMarketBackEnd.Utility;

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

    public Cart AddToCart(Cart cart)
    {
        var cartTemp = cart;
        var product = _context.Products.Find(cart.ProductId);
        if (product == null)
        {
            _logger.LogWarning("Product not found");
            throw new ApiException("Product not found",404);
        }
        if (cart.Quantity > product.Stock_quantity)
        {
            _logger.LogWarning("Not enough product");
            throw new ApiException("Not enough product",404);
        }
        cartTemp.Price = product.Price*cart.Quantity;;
        _context.Carts.Add(cartTemp);
        if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        {
            _logger.LogError("Error added cart");
            throw new Exception("Error added cart");
        }
        return cart;
    }

    public List<Cart> GetCartsByUserId(int userId)
    {
        var carts = _context.Carts.Where(c => c.UserId == userId).ToList();
        if (carts == null)
        {
            _logger.LogWarning("Carts not found");
            throw new ApiException("Carts not found",404);
        }

        return carts;
    }
    
    
    
}