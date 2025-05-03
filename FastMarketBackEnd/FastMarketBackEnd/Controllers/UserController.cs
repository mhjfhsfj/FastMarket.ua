using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using FastMarketBackEnd.services;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly UsersService _usersService;

        public UserController(ApplicationContext context, ILogger<UserController> logger, UsersService usersService)
        {
            _context = context;
            _logger = logger;
            _usersService = usersService;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/User
        [HttpGet(nameof(GetAllUsers))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _usersService.GetAllUsersAsync();
                if (users == null)
                {
                    return NotFound();
                }
                return users;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // GET: api/User/5
        [HttpGet(nameof(GetUserById)+"/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _usersService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChageUser)+"/{id}")]
        public async Task<IActionResult> ChageUser(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            try
            {
               await _usersService.ChangeUser(id, user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [HttpPut(nameof(ChangeUserByAdmin) + "/{id}")]
        public async Task<IActionResult> ChangeUserByAdmin(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            try
            {
                await _usersService.ChangeUserByAdmin(id, user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }

            return NoContent();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        // DELETE: api/User/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUser(int id)
        // {
        //     var user = await _context.Users.FindAsync(id);
        //     if (user == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.Users.Remove(user);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }
        
        //--------------------------------------------------------------------------------------------------------------

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
