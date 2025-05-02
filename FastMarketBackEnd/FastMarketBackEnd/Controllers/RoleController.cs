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
    public class RoleController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<RoleController> _logger;

        public RoleController(ApplicationContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Role
        [HttpGet(nameof(GetRoles))]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/Role/5
        [HttpGet(nameof(GetRoleById)+"/{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeRole)+"/{id}")]
        public async Task<IActionResult> ChangeRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateRole))]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleById", new { id = role.Id }, role);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/Role/5
        [HttpDelete(nameof(DeleteRole)+"/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
        
        //--------------------------------------------------------------------------------------------------------------
    }
}
