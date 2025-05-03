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
    public class ModerationController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ModerationController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Moderation
        [HttpGet(nameof(GetModerations))]
        public async Task<ActionResult<IEnumerable<Moderation>>> GetModerations()
        {
            return await _context.Moderations.Include(m=>m.Product)
                .Include(m=>m.User).ToListAsync();
        }

        // GET: api/Moderation/5
        [HttpGet(nameof(GetModerationById)+"/{id}")]
        public async Task<ActionResult<Moderation>> GetModerationById(int id)
        {
            var moderation = await _context.Moderations.Include(m=>m.Product)
                .Include(m=>m.User).FirstOrDefaultAsync(m=>m.Id == id);

            if (moderation == null)
            {
                return NotFound();
            }

            return moderation;
        }

        // PUT: api/Moderation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeModeration)+"/{id}")]
        public async Task<IActionResult> ChangeModeration(int id, Moderation moderation)
        {
            if (id != moderation.Id)
            {
                return BadRequest();
            }

            _context.Entry(moderation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModerationExists(id))
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

        // POST: api/Moderation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateModeration))]
        public async Task<ActionResult<Moderation>> CreateModeration(Moderation moderation)
        {
            _context.Moderations.Add(moderation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModerationById", new { id = moderation.Id }, moderation);
        }

        // DELETE: api/Moderation/5
        [HttpDelete(nameof(DeleteModeration)+"/{id}")]
        public async Task<IActionResult> DeleteModeration(int id)
        {
            var moderation = await _context.Moderations.FindAsync(id);
            if (moderation == null)
            {
                return NotFound();
            }

            _context.Moderations.Remove(moderation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModerationExists(int id)
        {
            return _context.Moderations.Any(e => e.Id == id);
        }
    }
}
