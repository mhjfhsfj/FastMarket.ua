using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;
using FastMarketBackEnd.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastMarketBackEnd.services
{
    
    public class UsersService
    {
        private readonly ApplicationContext db;
        private readonly ILogger<UsersService> logger;

        public UsersService(ApplicationContext db, ILogger<UsersService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await this.db.Users.Select(u => u).ToListAsync();

            var usersList = new List<UserDto>();

            foreach (var user in users)
            {
                usersList.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.email
                });
            }

            return usersList;
        }
    }
}