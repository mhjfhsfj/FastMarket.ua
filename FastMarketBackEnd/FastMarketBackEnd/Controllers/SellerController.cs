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

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<SellerController> _logger;
        private readonly UsersService _usersService;

        public SellerController(ApplicationContext context, ILogger<SellerController> logger, UsersService usersService)
        {
            _context = context;
            _logger = logger;
            _usersService = usersService;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Seller
        [HttpGet(nameof(GetSellers))]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            try
            {
                var sellers = _usersService.GetSeller();
                return sellers;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
            
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Seller/5
        [HttpGet(nameof(GetSellerById)+"/{id}")]
        public async Task<ActionResult<Seller>> GetSellerById(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
        
            if (seller == null)
            {
                return NotFound();
            }
        
            return seller;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        // PUT: api/Seller/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeSeller)+"/{id}")]
        public async Task<IActionResult> ChangeSeller(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }
        
            _context.Entry(seller).State = EntityState.Modified;
        
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        
            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // POST: api/Seller
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateSeller))]
        public async Task<ActionResult<Seller>> CreateSeller(Seller seller)
        {
            try
            {
                if (seller != null)
                {
                    var _seller = _usersService.CreateSeller(seller);
                    return Ok(_seller);
                }

                throw new Exception("Помилковий запит");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Seller/5
        [HttpDelete(nameof(DeleteSeller)+"/{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
        
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
