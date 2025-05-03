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
        private readonly ILogger<UsersService> _logger;

        public UsersService(ApplicationContext context, ILogger<UsersService> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await this._context.Users.Select(u => u).ToListAsync();

            

            var usersList = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                SecondName = u.SecondName,
                LastName = u.LastName,
                email = u.email,
                phone = u.phone,
                Role = u.Role,
                Seller = new SellerDTO
                {
                    Id = u.Seller.Id,
                    Name = u.Seller.Name,
                    Description = u.Seller.Description
                }
            }).ToList();

            // foreach (var user in users)
            // {
            //     usersList.Add(new UserDto
            //     {
            //         Id = user.Id,
            //         Name = user.Name,
            //         SecondName = user.SecondName,
            //         LastName = user.LastName,
            //         email = user.email,
            //         phone = user.phone,
            //         Role = user.Role,
            //         Seller = new SellerDTO
            //         {
            //             Id = user.Seller.Id,
            //             Name = user.Seller.Name,
            //             Description = user.Seller.Description
            //         }
            //         
            //     });
            // }

            return usersList;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await this._context.Users.Include(u=>u.Seller).FirstOrDefaultAsync(u => u.Id == id);
            if (user==null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }

            var userDTO = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                SecondName = user.SecondName,
                LastName = user.LastName,
                email = user.email,
                phone = user.phone,
                Role = user.Role,
                Seller = new SellerDTO
                {
                    Id = user.Seller.Id,
                    Name = user.Seller.Name,
                    Description = user.Seller.Description
                }
            };

            return userDTO;
        }

        public async Task ChangeUser(int id, UserDto userDTO)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }
            user.Name = userDTO.Name;
            user.SecondName = userDTO.SecondName;
            user.LastName = userDTO.LastName;
            user.email = userDTO.email;
            user.phone = userDTO.phone;
            
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated");
            
        }
        
        public async Task ChangeUserByAdmin(int id, UserDto userDTO)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }
            user.Name = userDTO.Name;
            user.SecondName = userDTO.SecondName;
            user.LastName = userDTO.LastName;
            user.email = userDTO.email;
            user.phone = userDTO.phone;
            user.Role = userDTO.Role;
            
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated");
            
        }

        public Seller CreateSeller(Seller seller)
        {
            var _seller = this._context.Sellers.Add(seller);
            this._context.SaveChanges();
            _logger.LogInformation("Seller created");
            return _seller.Entity;
        }

        public async Task<List<Seller>> GetSellers()
        {
            var sellers = this._context.Sellers.Include(u=>u.User).ToList();
            
            if (sellers == null)
            {
                _logger.LogError("Sellers not found");
                throw new Exception("Sellers not found");
            }
            return sellers;
        }

        public Seller GetSellerById(int id)
        {
            var seller = this._context.Sellers.Include(u=>u.User).FirstOrDefault(u=>u.Id == id);
            if (seller == null)
            {
                _logger.LogError("Seller not found");
                throw new Exception("Seller not found");
            }
            return seller;
        }

        public Seller GetSellerByUserId(int id)
        {
            var seller = this._context.Sellers.Include(u=>u.User).FirstOrDefault(u=>u.UserId == id);
            if (seller == null)
            {
                _logger.LogError("Seller not found");
                throw new Exception("Seller not found");
            }
            return seller;
        }

        public Seller UpdateSeller(Seller seller)
        {
            var _seller = this._context.Sellers.Update(seller);
            this._context.SaveChanges();
            _logger.LogInformation("Seller updated");
            return _seller.Entity;
        }
        
        public async Task DeleteSeller(int id)
        {
            var seller = await this._context.Sellers.FirstOrDefaultAsync(u=>u.Id == id);
            if (seller == null)
            {
                _logger.LogError("Seller not found");
                throw new Exception("Seller not found");
            }
            
        }
    }
}