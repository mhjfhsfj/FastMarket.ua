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
    public class RatingController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<RatingController> _logger;

        public RatingController(ApplicationContext context, ILogger<RatingController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Rating
        [HttpGet(nameof(GetRatings))]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
        {
            return await _context.Ratings.ToListAsync();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Rating/5
        [HttpGet(nameof(GetRatingById)+"/{id}")]
        public async Task<ActionResult<Rating>> GetRatingById(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            return rating;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Rating/5
        [HttpGet(nameof(GetRatingByProductIdUserId)+"/{productId}/{userId}")]
        public async Task<ActionResult<Rating>> GetRatingByProductIdUserId(int productId,int userId)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if (rating == null)
            {
                return NotFound();
            }

            return rating;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Rating/5
        [HttpGet(nameof(GetRatingsByProductId)+"/{id}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsByProductId(int id)
        {
            var rating = await _context.Ratings.Where(r => r.ProductId == id).ToListAsync();

            if (rating == null)
            {
                _logger.LogInformation("Not Found ratings");
                return NotFound();
            }

            return rating;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/Rating/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeRating)+"/{id}")]
        public async Task<IActionResult> ChangeRating(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                return BadRequest();
            }
            
            if (rating.Score < 1 || rating.Score > 5)
            {
                throw new Exception("Error score, must be between 1 and 5");
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                
                await _context.SaveChangesAsync();
                await RatingAverageScore(rating.ProductId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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

        // POST: api/Rating
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateRating))]
        public async Task<ActionResult<Rating>> CreateRating(Rating rating)
        {
            try
            {
                
                if (rating.Score < 1 || rating.Score > 5)
                {
                    throw new Exception("Error score, must be between 1 and 5");
                }

                if (await _context.Ratings.FirstOrDefaultAsync(r =>
                        r.UserId == rating.UserId && r.ProductId == rating.ProductId) != null)
                {
                    throw new Exception("score is already there");
                }

                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();
                
                await RatingAverageScore(rating.ProductId);

                return CreatedAtAction("GetRatingById", new { id = rating.Id }, rating);
            }
            catch (Exception e)
            {
             _logger.LogError(e.Message);   
             return BadRequest(e.Message);   
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Rating/5
        [HttpDelete(nameof(DeleteRating)+"/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool RatingExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }

        private async Task RatingAverageScore(int productId)
        {
            var ratings = _context.Ratings.Where(r=>r.ProductId==productId).ToList();
            var product = _context.Products.FirstOrDefaultAsync(p => p.Id == productId).Result;
            int sum=0;
            int number = 0;
            foreach (var rating in ratings)
            {
                sum += rating.Score;
                number++;
            }

            product.AverageScore = (decimal)sum / number;
            
            _context.Entry(product).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error score: {e.Message}");
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
    }
}
