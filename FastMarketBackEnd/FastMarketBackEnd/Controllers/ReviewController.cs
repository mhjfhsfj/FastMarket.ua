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
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Review
        [HttpGet(nameof(GetReviews))]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            try
            {
                var Reviews = await _context.Reviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .ToListAsync();
                if (Reviews == null)
                {
                    _logger.LogError("Reviews not found");
                    return NotFound();
                }
                return Reviews;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Review/5
        [HttpGet(nameof(GetReviewById)+"/{id}")]
        public async Task<ActionResult<Review>> GetReviewById(int id)
        {
            var review = await _context.Reviews
                .Include(r=>r.Product)
                .Include(r=>r.User)
                .FirstOrDefaultAsync(r=>r.Id == id);

            if (review == null)
            {
                _logger.LogError("Review not found. Id: " + id + "Review");
                return NoContent();
            }

            return review;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/Review/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeReview)+"/{id}")]
        public async Task<IActionResult> ChangeReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Review
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateReview))]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewById", new { id = review.Id }, review);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Review/5
        [HttpDelete(nameof(DeleteReview)+"/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
        
        //--------------------------------------------------------------------------------------------------------------
    }
}
