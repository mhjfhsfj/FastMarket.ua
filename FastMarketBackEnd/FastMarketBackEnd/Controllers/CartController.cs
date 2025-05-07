using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;
using FastMarketBackEnd.services;
using FastMarketBackEnd.Utility;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CartController> _logger;
        private readonly CartService _cartService;

        public CartController(ApplicationContext context, ILogger<CartController> logger, CartService cartService)
        {
            _context = context;
            _logger = logger;
            _cartService = cartService;
        }

        // GET: api/Cart
        [HttpGet(nameof(GetCarts))]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            try
            {
                return await _cartService.GetCartsAsync();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        // GET: api/Cart/GetCartById/5
        [HttpGet(nameof(GetCartById)+"/{id}")]
        public async Task<ActionResult<Cart>> GetCartById(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id);
                return cart;
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet(nameof(GetCartByUserId) + "/{userId}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartByUserId(int userId)
        {
            try
            {
                var carts = await _cartService.GetCartsByUserIdAsync(userId);
                return carts;
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeCart)+"/{id}")]
        public async Task<IActionResult> ChangeCart(int id, Cart cart)
        {
            try
            {
                await _cartService.ChangeCartAsync(id, cart);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            
        }

        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(AddCart))]
        public async Task<ActionResult<Cart>> AddCart(Cart cart)
        {
            try
            {
                var _cart = await _cartService.AddToCartAsync(cart);
                return _cart;
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Cart/5
        [HttpDelete(nameof(DeleteCart)+"/{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            try
            {
                await _cartService.DeleteCartAsync(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete(nameof(DeleteCartsByUserId) + "/{userId}")]
        public async Task<IActionResult> DeleteCartsByUserId(int userId)
        {
            try
            {
                await _cartService.DeleteCartsByUserIdAsync(userId);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
