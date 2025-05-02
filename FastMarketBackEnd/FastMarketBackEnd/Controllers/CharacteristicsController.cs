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
    public class CharacteristicsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CharacteristicsController> _logger;

        public CharacteristicsController(ApplicationContext context, ILogger<CharacteristicsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Characteristics
        [HttpGet(nameof(GetNameCharacteristics))]
        public async Task<ActionResult<IEnumerable<NameCharacteristics>>> GetNameCharacteristics()
        {
            return await _context.NameCharacteristics.ToListAsync();
        }

        // GET: api/Characteristics/5
        // [HttpGet("{id}")]
        [HttpGet(nameof(GetByIdNameCharacteristics))]
        public async Task<ActionResult<NameCharacteristics>> GetByIdNameCharacteristics(int id)
        {
            var nameCharacteristics = await _context.NameCharacteristics.FindAsync(id);

            if (nameCharacteristics == null)
            {
                return NotFound();
            }

            return nameCharacteristics;
        }
        
        // GET: api/Characteristics/5
        // [HttpGet("{id}")]
        [HttpGet(nameof(GetByCategoryIdNameCharacteristics))]
        public async Task<ActionResult<IEnumerable<NameCharacteristics>>> GetByCategoryIdNameCharacteristics(int id)
        {
            var nameCharacteristics = await _context.NameCharacteristics.Include(c=>c.Category).Where(c=>c.CategoryId == id).ToListAsync();

            if (nameCharacteristics == null)
            {
                return NotFound();
            }

            return nameCharacteristics;
        }

        // PUT: api/Characteristics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(PutNameCharacteristics))]
        public async Task<IActionResult> PutNameCharacteristics(int id, NameCharacteristics nameCharacteristics)
        {
            if (id != nameCharacteristics.Id)
            {
                return BadRequest();
            }

            _context.Entry(nameCharacteristics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NameCharacteristicsExists(id))
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

        // POST: api/Characteristics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(PostNameCharacteristics))]
        public async Task<ActionResult<NameCharacteristics>> PostNameCharacteristics(NameCharacteristics nameCharacteristics)
        {
            _context.NameCharacteristics.Add(nameCharacteristics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNameCharacteristics", new { id = nameCharacteristics.Id }, nameCharacteristics);
        }

        // DELETE: api/Characteristics/5
        [HttpDelete(nameof(DeleteByIdNameCharacteristics))]
        public async Task<IActionResult> DeleteByIdNameCharacteristics(int id)
        {
            var nameCharacteristics = await _context.NameCharacteristics.FindAsync(id);
            if (nameCharacteristics == null)
            {
                return NotFound();
            }

            _context.NameCharacteristics.Remove(nameCharacteristics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NameCharacteristicsExists(int id)
        {
            return _context.NameCharacteristics.Any(e => e.Id == id);
        }
        
        //______________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________
        
        // GET: api/Characteristics
        [HttpGet(nameof(GetCharacteristics))]
        public async Task<ActionResult<IEnumerable<NameCharacteristics>>> GetCharacteristics()
        {
            return await _context.NameCharacteristics.ToListAsync();
        }
        //______________________________________________________________________________________________________________

        // GET: api/Characteristics/5
        // [HttpGet("{id}")]
        [HttpGet(nameof(GetByIdCharacteristics)+"/{id}")]
        public async Task<ActionResult<NameCharacteristics>> GetByIdCharacteristics(int id)
        {
            var nameCharacteristics = await _context.NameCharacteristics.FindAsync(id);

            if (nameCharacteristics == null)
            {
                return NotFound();
            }

            return nameCharacteristics;
        }
        //______________________________________________________________________________________________________________

        // PUT: api/Characteristics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeCharacteristics)+"/{id}")]
        public async Task<IActionResult> ChangeCharacteristics(int id, Characteristics characteristics)
        {
            if (id != characteristics.Id)
            {
                return BadRequest();
            }

            _context.Entry(characteristics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacteristicsExists(id))
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
        //______________________________________________________________________________________________________________

        // POST: api/Characteristics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateCharacteristics))]
        public async Task<ActionResult<NameCharacteristics>> CreateCharacteristics(Characteristics characteristics)
        {
            _context.Characteristics.Add(characteristics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacteristics", new { id = characteristics.Id }, characteristics);
        }
        //______________________________________________________________________________________________________________

        // DELETE: api/Characteristics/5
        [HttpDelete(nameof(DeleteByIdCharacteristics)+"/{id}")]
        public async Task<IActionResult> DeleteByIdCharacteristics(int id)
        {
            var characteristics = await _context.Characteristics.FindAsync(id);
            if (characteristics == null)
            {
                return NotFound();
            }

            _context.Characteristics.Remove(characteristics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacteristicsExists(int id)
        {
            return _context.Characteristics.Any(e => e.Id == id);
        }
    }
}
