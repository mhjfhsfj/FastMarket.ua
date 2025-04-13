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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationContext context,ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Сategory>>> GetCategories()
        {
            return await _context.Categories.Include(c=>c.PrimaryCategory).ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Сategory>> GetСategory(int id)
        {
            var сategory = await _context.Categories.FindAsync(id);

            if (сategory == null)
            {
                return NotFound();
            }

            return сategory;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutСategory(int id, Сategory сategory)
        {
            if (id != сategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(сategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!СategoryExists(id))
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

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Сategory>> PostСategory(Сategory сategory)
        {
            try
            {
                if (сategory.Name != null)
                {
                    _context.Categories.Add(сategory);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetСategory", new { id = сategory.Id }, сategory);
                }
                // this._logger.LogError("Не вказана категорія");
                throw new Exception("Не вказана категорія");
            }
            catch(Exception e)
            {
                this._logger.LogError($"Помилка запиту {e.Message}");
                return BadRequest($"Помилка запиту {e.Message}");
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteСategory(int id)
        {
            var сategory = await _context.Categories.FindAsync(id);
            if (сategory == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(сategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool СategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
