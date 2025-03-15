using FastMarketBackEnd.Data;
using FastMarketBackEnd.models;
using FastMarketBackEnd.DataTypes;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.services;
using FastMarketBackEnd.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastMarketBackEnd.services
{
    public class AuthService
{
    private readonly ApplicationContext db;
    private readonly TokensService tokensService;
    private readonly ILogger<AuthService> logger;

    public AuthService(ApplicationContext db, TokensService tokensService, ILogger<AuthService> logger)
    {
        this.db = db;
        this.tokensService = tokensService;
        this.logger = logger;
    }

    // public async Task<UserData> RegistrationAsync(AuthDto request, UserAgentData userAgentData)
    // {
    //     var user = await this.db.Users.FirstOrDefaultAsync(u => u.email.Equals(request.Email));
    //     if (user is not null)
    //     {
    //         this.logger.LogError("Error register, user with such an email already exists!");
    //         throw new Exception("Не удалось зарегистрироваться, пользователь с таким Email уже существует!");
    //     }
    //
    //     var hashPassword = Utilities.Hash(request.Password);
    //
    //     this.logger.LogInformation("Add User data");
    //
    //     var userEntry = await this.db.Users.AddAsync(new User
    //     {
    //         email = request.Email,
    //         password_hash = hashPassword
    //     });
    //
    //     await this.db.SaveChangesAsync();
    //
    //     var addedUserId = userEntry.Entity.Id;
    //     var userDto = new UserDto { Id = addedUserId, Email = request.Email };
    //
    //     this.logger.LogInformation("Generate tokens");
    //     var tokens = this.tokensService.GenerateTokens(userDto);
    //
    //     this.logger.LogInformation("Save refresh token");
    //     await this.tokensService.SaveRefreshTokenAsync(addedUserId, tokens.RefreshJwt, userAgentData);
    //
    //     var userData = new UserData { UserDto = userDto, TokensData = tokens };
    //
    //     return userData;
    // }
    
    public async Task<UserDataAlt> RegistrationAndLoginByPhoneAsync(UserPhoneCodeDTO request, UserAgentData userAgentData)
    {
        var user = await this.db.Users.FirstOrDefaultAsync(u => u.phone.Equals(request.PhoneNumber));
        if (user is not null)
        {
            this.logger.LogInformation("Start login");

            this.logger.LogInformation("Add User data");

            var userPhoneDto = new UserPhoneDTO() { Id = user.Id, Phone = user.phone };
    
            this.logger.LogInformation("Generate tokens");
            var tokens = this.tokensService.GenerateTokens(userPhoneDto);
    
            this.logger.LogInformation("Save refresh token");
            await this.tokensService.SaveRefreshTokenAsync(user.Id, tokens.RefreshJwt, userAgentData);

            var userDataAlt = new UserDataAlt() { UserPhoneDto = userPhoneDto, TokensData = tokens };

            return userDataAlt;
        }
        else
        {
            // var hashPassword = Utilities.Hash(request.Password);
            this.logger.LogInformation("Start register");
            this.logger.LogInformation("Add User data");

            var userEntry = await this.db.Users.AddAsync(new User
            {
                phone = request.PhoneNumber,
                
            });

            await this.db.SaveChangesAsync();

            var addedUserId = userEntry.Entity.Id;
            var userPhoneDto = new UserPhoneDTO() { Id = addedUserId, Phone = request.PhoneNumber };

            this.logger.LogInformation("Generate tokens");
            var tokens = this.tokensService.GenerateTokens(userPhoneDto);

            this.logger.LogInformation("Save refresh token");
            await this.tokensService.SaveRefreshTokenAsync(addedUserId, tokens.RefreshJwt, userAgentData);

            var userDataAlt = new UserDataAlt() { UserPhoneDto = userPhoneDto, TokensData = tokens };
            return userDataAlt;
        }
    }

    // public async Task<UserData> LoginAsync(AuthDto request, UserAgentData userAgentData)
    // {
    //     var user = await this.db.Users.FirstOrDefaultAsync(u => u.email.Equals(request.Email));
    //     if (user is null)
    //     {
    //         this.logger.LogError("User with such email is not found!");
    //         throw new Exception("Пользователь с таким Email не найден!");
    //     }
    //
    //     if (!user.password_hash.Equals(Utilities.Hash(request.Password)))
    //     {
    //         this.logger.LogError("Invalid password!");
    //         throw new Exception("Неверный пароль!");
    //     }
    //
    //     var userDto = new UserDto { Id = user.Id, Email = user.email };
    //
    //     this.logger.LogInformation("Generate tokens");
    //     var tokens = this.tokensService.GenerateTokens(userDto);
    //
    //     this.logger.LogInformation("Save refresh token");
    //     await this.tokensService.SaveRefreshTokenAsync(user.Id, tokens.RefreshJwt, userAgentData);
    //
    //     var userData = new UserData { UserDto = userDto, TokensData = tokens };
    //
    //     return userData;
    // }

    public async Task<string> LogoutAsync(string refreshToken)
    {
        this.logger.LogInformation("Remove refresh token");
        var removedRefreshToken = await this.tokensService.RemoveRefreshTokenAsync(refreshToken);
        return removedRefreshToken;
    }
}
}
