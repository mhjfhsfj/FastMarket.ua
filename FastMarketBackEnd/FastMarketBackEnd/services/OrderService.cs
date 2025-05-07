using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using FastMarketBackEnd.Utility;
using Microsoft.EntityFrameworkCore;

namespace FastMarketBackEnd.services;

public class OrderService
{
    private readonly ApplicationContext _context;
    private readonly ILogger<OrderService> _logger;
    
    public OrderService(ApplicationContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<GetOrderDTO>> CreateOrderAsync(CreateOrderRequest сreateOrderRequest)
    {
        var user = _context.Users.FindAsync(сreateOrderRequest.BuyerId);
        if (user == null)
        {
            _logger.LogWarning("User not found");
            throw new NotFoundException("User not found",404);
        }
        var productIds = сreateOrderRequest.Products.Select(i => i.ProductId).ToList();

        // Завантажуємо продукти з бази разом з продавцями
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.Seller)
            .ToListAsync();

        // Перевірка — чи всі продукти існують
        if (products.Count != productIds.Count)
            throw new Exception("Деякі продукти не знайдено");

        // Групуємо по продавцю
        var groupedBySeller = сreateOrderRequest.Products
            .Join(products, ci => ci.ProductId, p => p.Id, (ci, p) => new { CartItem = ci, Product = p })
            .GroupBy(x => x.Product.SellerId);

        var createdOrders = new List<Order>();

        foreach (var group in groupedBySeller)
        {
            foreach (var item in group)
            {
                if (item.CartItem.Quantity > item.Product.Stock_quantity)
                {
                    _logger.LogWarning($"Not enough product: {item.Product}");
                    throw new NotFoundException($"Not enough product: {item.Product}",404);
                }
            }
            var order = new Order
            {
                UserId = сreateOrderRequest.BuyerId,
                SellerId = group.Key,
                CreatedAt = DateTime.UtcNow,
                StatusOsrder = StatusOrder.Pending,
                DeliveryAddress = сreateOrderRequest.DeliveryAddress,
                Phone = сreateOrderRequest.Phone,
                PaymentMethod = сreateOrderRequest.PaymentMethod,
                DeliveryMethod = сreateOrderRequest.DeliveryMethod,
                PaymentStatus = PaymentStatus.Pending,
                OrderDetails = group.Select(x => new OrderDetails
                {
                    ProductId = x.Product.Id,
                    Quantity = x.CartItem.Quantity,
                    Price = x.Product.Price
                }).ToList()
            };
            order.TotalPrice = order.OrderDetails.Sum(x => x.TotalPrice);
            _context.Orders.Add(order);
            createdOrders.Add(order);
        }
        
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error added order");
        //     throw new Exception("Error added order");
        // }
        await _context.SaveChangesAsync();
        
        var orderDtos = new List<GetOrderDTO>();
        foreach (var order in createdOrders)
        {
            var orderDto = new GetOrderDTO
            {
                Id = order.Id,
                User = order.User,
                Seller = order.Seller,
                DeliveryAddress = order.DeliveryAddress,
                Phone = order.Phone,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails,
                StatusOsrder = order.StatusOsrder,
                PaymentMethod = order.PaymentMethod,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
                
            };
            orderDtos.Add(orderDto);
        }
        
        _logger.LogInformation("Add order successfully");
        return orderDtos;
    }

    public async Task<List<GetOrderDTO>> GetOrdersByUserIdAsync(int userId)
    {
        _logger.LogInformation("Get orders by user id start");
        var orders = await _context.Orders
            .Include(o=>o.OrderDetails)
            .Where(o => o.UserId == userId).ToListAsync();
        if (orders == null)
        {
            _logger.LogWarning("Orders not found");
            throw new NotFoundException("Orders not found",404);
        }
        var orderDtos = new List<GetOrderDTO>();
        foreach (var order in orders)
        {
            var orderDto = new GetOrderDTO
            {
                Id = order.Id,
                User = order.User,
                Seller = order.Seller,
                DeliveryAddress = order.DeliveryAddress,
                Phone = order.Phone,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails,
                StatusOsrder = order.StatusOsrder,
                PaymentMethod = order.PaymentMethod,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
            };
            orderDtos.Add(orderDto);
        }
        _logger.LogInformation("Get orders by user id successfully");
        return orderDtos;
    }

    public async Task<List<GetOrderDTO>> GetOrdersBySellerIdAsync(int sellerId)
    {
        _logger.LogInformation("Get orders by seller id start");
        var orders = await _context.Orders
            .Include(o=>o.OrderDetails)
            .Where(o => o.SellerId == sellerId).ToListAsync();
        if (orders == null)
        {
            _logger.LogWarning("Orders not found");
            throw new NotFoundException("Orders not found",404);
        }
        var orderDtos = new List<GetOrderDTO>();
        foreach (var order in orders)
        {
            var orderDto = new GetOrderDTO
            {
                Id = order.Id,
                User = order.User,
                Seller = order.Seller,
                DeliveryAddress = order.DeliveryAddress,
                Phone = order.Phone,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails,
                StatusOsrder = order.StatusOsrder,
                PaymentMethod = order.PaymentMethod,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
            };
            orderDtos.Add(orderDto);
        }
        _logger.LogInformation("Get orders by user id successfully");
        return orderDtos;
    }
   
    public async Task<List<GetOrderDTO>> GetOrdersAsync()
    {
        _logger.LogInformation("Get orders start");
        var orders = await _context.Orders
            .Include(o=>o.OrderDetails)
            .ToListAsync();
        if (orders == null)
        {
            _logger.LogWarning("Orders not found");
            throw new NotFoundException("Orders not found",404);
        }
        var orderDtos = new List<GetOrderDTO>();
        foreach (var order in orders)
        {
            var orderDto = new GetOrderDTO
            {
                Id = order.Id,
                User = order.User,
                Seller = order.Seller,
                DeliveryAddress = order.DeliveryAddress,
                Phone = order.Phone,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails,
                StatusOsrder = order.StatusOsrder,
                PaymentMethod = order.PaymentMethod,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
            };
            orderDtos.Add(orderDto);
        }
        _logger.LogInformation("Get orders by user id successfully");
        return orderDtos;
    }
    public async Task<GetOrderDTO> GetOrderByIdAsync(int id)
    {
        _logger.LogInformation("Get order by id start");
        var order = await _context.Orders
            .Include(o=>o.OrderDetails)
            .Where(o => o.Id == id).FirstOrDefaultAsync();
        if (order == null)
        {
            _logger.LogWarning("Order not found");
            throw new NotFoundException("Order not found",404);
        }
        var orderDto = new GetOrderDTO
        {
            Id = order.Id,
            User = order.User,
            Seller = order.Seller,
            DeliveryAddress = order.DeliveryAddress,
            Phone = order.Phone,
            TotalPrice = order.TotalPrice,
            OrderDetails = order.OrderDetails,
            StatusOsrder = order.StatusOsrder,
            PaymentMethod = order.PaymentMethod,
            DeliveryMethod = order.DeliveryMethod,
            PaymentStatus = order.PaymentStatus,
        };
        return orderDto;
    }

    public async Task ChangeOrderAsync(int id, OrderChangeDTO order)
    {
        _logger.LogInformation("Change order start");
        if (id != order.Id)
        {
            _logger.LogWarning("Id not match");
            throw new NotFoundException("Id not match",404);
        }
        var orderTemp = await _context.Orders.FindAsync(id);
        if (orderTemp == null)
        {
            _logger.LogError($"Order not exist id:{id}");
        }
        orderTemp.Phone = order.Phone;
        orderTemp.DeliveryAddress = order.DeliveryAddress;
        // orderTemp.StatusOsrder = order.StatusOsrder;
        orderTemp.OrderDetails = order.OrderDetails;

        _context.Entry(orderTemp).State = EntityState.Modified;
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error change order");
        //     throw new Exception("Error change order");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Change order end");
    }

    public async Task ChangeOrderStatusAsync(int id, StatusOrder statusOsrder)
    {
        _logger.LogInformation("Change status start");
        var orderTemp = await _context.Orders.FindAsync(id);
        if (id != orderTemp.Id)
        {
            _logger.LogWarning("Id not match");
            throw new NotFoundException("Id not match",404);
        }

        if (statusOsrder == StatusOrder.Shipped)
        {
            foreach (var orderDetail in orderTemp.OrderDetails)
            {
                var product = await _context.Products.FindAsync(orderDetail.ProductId);
                product.Stock_quantity -= orderDetail.Quantity;
                if (product.Stock_quantity < 0)
                {
                    _logger.LogWarning($"Not enough product: {product}");
                    throw new NotFoundException($"Not enough product: {product}",404);
                }
                _context.Entry(product).State = EntityState.Modified;
                // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
                // {
                //     _logger.LogError("Error change order");
                //     throw new Exception("Error change order");
                // }
                await _context.SaveChangesAsync();
            }
        }
        orderTemp.StatusOsrder = statusOsrder;
        _context.Entry(orderTemp).State = EntityState.Modified;
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error change order");
        //     throw new Exception("Error change order");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Change status end");
    }
    
    public async Task ChangeOrderPaymentStatusAsync(int id, PaymentStatus paymentStatus)
    {
        _logger.LogInformation("Change status start");
        var orderTemp = await _context.Orders.FindAsync(id);
        if (id != orderTemp.Id)
        {
            _logger.LogWarning("Id not match");
            throw new NotFoundException("Id not match",404);
        }
        orderTemp.PaymentStatus = paymentStatus;
        _context.Entry(orderTemp).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Change status end");
    }
    
    public async Task DeleteOrderAsync(int id)
    {
        _logger.LogInformation("Delete order start");
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            _logger.LogWarning("Order not found");
            throw new NotFoundException("Order not found",404);
        }
        _context.Orders.Remove(order);
        // if (!_context.SaveChangesAsync().IsCompletedSuccessfully)
        // {
        //     _logger.LogError("Error delete order");
        //     throw new Exception("Error delete order");
        // }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Delete order successfully");
    }
    
}