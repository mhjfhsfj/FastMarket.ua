using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastMarketBackEnd.services
{
    
    public class UsersService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UsersService> logger;

        public UsersService(ApplicationContext context, ILogger<UsersService> logger)
        {
            this._context = context;
            this.logger = logger;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await this._context.Users.Select(u => u).ToListAsync();

            var usersList = new List<UserDto>();

            foreach (var user in users)
            {
                usersList.Add(new UserDto
                {
                    Id = user.Id,
                    phone = user.phone
                });
            }

            return usersList;
        }

        public Seller CreateSeller(Seller seller)
        {
            var _seller = this._context.Sellers.Add(seller);
            this._context.SaveChanges();
            return _seller.Entity;
        }

        public List<Seller> GetSeller()
        {
            var sellers = this._context.Sellers.Include(u=>u.User).ToList();
                
            return sellers;
        }
    }
}