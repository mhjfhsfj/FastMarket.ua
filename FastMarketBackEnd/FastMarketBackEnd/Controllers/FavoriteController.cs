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
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(ApplicationContext context, ILogger<FavoriteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //--------------------------------------------------------------------------------------------------------------
        
        // GET: api/Favorite
        [HttpGet(nameof(GetFavorites))]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorites()
        {
            return await _context.Favorites
                .Include(f=>f.Product)
                .Include(f=>f.User)
                .ToListAsync();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Favorite/5
        [HttpGet(nameof(GetFavoriteById)+"/{id}")]
        public async Task<ActionResult<Favorite>> GetFavoriteById(int id)
        {
            var favorite = await _context.Favorites
                .Include(f=>f.User)
                .Include(f=>f.Product)
                .FirstOrDefaultAsync(f=>f.Id == id);

            if (favorite == null)
            {
                return NotFound();
            }

            return favorite;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/Favorite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(PutFavorite) + "/{id}")]
        public async Task<IActionResult> PutFavorite(int id, Favorite favorite)
        {
            if (id != favorite.Id)
            {
                return BadRequest();
            }

            _context.Entry(favorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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

        // POST: api/Favorite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateFavorite))]
        public async Task<ActionResult<Favorite>> CreateFavorite(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavoriteById", new { id = favorite.Id }, favorite);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Favorite/5
        [HttpDelete(nameof(DeleteFavorite)+"/{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool FavoriteExists(int id)
        {
            return _context.Favorites.Any(e => e.Id == id);
        }
    }
}
