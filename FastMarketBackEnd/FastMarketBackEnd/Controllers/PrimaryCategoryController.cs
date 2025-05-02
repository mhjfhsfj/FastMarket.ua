using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryCategoryController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<PrimaryCategoryController> _logger;

        public PrimaryCategoryController(ApplicationContext context, ILogger<PrimaryCategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //--------------------------------------------------------------------------------------------------------------
        // GET: api/Category
        [HttpGet(nameof(GetPrimaryСategories))]
        public async Task<ActionResult<IEnumerable<PrimaryСategory>>> GetPrimaryСategories()
        {
            var primaryCategory = await _context.PrimaryСategories.ToListAsync();
            
            return primaryCategory;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Category/5
        [HttpGet(nameof(GetPrimaryСategoryById)+"/{id}")]
        public async Task<ActionResult<PrimaryСategory>> GetPrimaryСategoryById(int id)
        {
            var primaryСategory = await _context.PrimaryСategories.FindAsync(id);

            if (primaryСategory == null)
            {
                return NotFound();
            }

            return primaryСategory;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangePrimaryСategory)+"/{id}")]
        public async Task<IActionResult> ChangePrimaryСategory(int id, PrimaryСategory primaryСategory)
        {
            if (id != primaryСategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(primaryСategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrimaryСategoryExists(id))
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

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreatePrimaryСategory))]
        public async Task<ActionResult<PrimaryСategory>> CreatePrimaryСategory(PrimaryСategory primaryСategory)
        {
            try
            {
                if (primaryСategory.Name != null)
                {
                    var _primaryCategory = this._context.PrimaryСategories.Add(primaryСategory);
                    await _context.SaveChangesAsync();
                    return _primaryCategory.Entity;
                }
                throw new Exception("Не вказана категорія");
            }
            catch (Exception e)
            {
                this._logger.LogError($"Помилка запиту: {e.Message}");
                return BadRequest($"Помилка запиту: {e.Message}");
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Category/5
        [HttpDelete(nameof(DeletePrimaryСategory)+"/{id}")]
        public async Task<IActionResult> DeletePrimaryСategory(int id)
        {
            var primaryСategory = await _context.PrimaryСategories.FindAsync(id);
            if (primaryСategory == null)
            {
                return NotFound();
            }

            _context.PrimaryСategories.Remove(primaryСategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool PrimaryСategoryExists(int id)
        {
            return _context.PrimaryСategories.Any(e => e.Id == id);
        }
    }
}
