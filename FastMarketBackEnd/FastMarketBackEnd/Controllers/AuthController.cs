using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.DataTypes;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using FastMarketBackEnd.services;
using FastMarketBackEnd.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly TokensService tokensService;
        private readonly AuthService authService;
        private readonly SmsSevice smsService;
        private readonly ILogger<AuthController> logger;
        private readonly string refreshTokenKey;

        /// <inheritdoc />
        public AuthController(
            TokensService tokensService,
            AuthService authService,
            SmsSevice smsSevice,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            this.tokensService = tokensService;
            this.authService = authService;
            this.smsService = smsSevice;
            this.logger = logger;
            this.refreshTokenKey = configuration.GetSection("CookiesNames").GetChildren().First(c => c.Key.Equals("RefreshToken")).Value;
        }

        // [AllowAnonymous]
        // [HttpPost(nameof(Registration))]
        // public async Task<ActionResult<UserData>> Registration([FromBody] AuthDto request)
        // {
        //     try
        //     {
        //         var userAgentData = Utilities.GetUserAgentData(this.Request.Headers["User-Agent"]);
        //
        //         this.logger.LogInformation("Start registration");
        //         var userData = await this.authService.RegistrationAsync(request, userAgentData);
        //
        //         this.logger.LogInformation("Add refresh token cookie");
        //         this.AddRefreshTokenCookie(new Token(), userData);
        //
        //         return this.Ok(userData);
        //     }
        //     catch (Exception e)
        //     {
        //         return this.BadRequest($"Ошибка при регистрации:{e.Message}");
        //     }
        // }
        //
        // [AllowAnonymous]
        // [HttpPost(nameof(Login))]
        // public async Task<ActionResult<UserData>> Login([FromBody] AuthDto request)
        // {
        //     try
        //     {
        //         var userAgentData = Utilities.GetUserAgentData(this.Request.Headers["User-Agent"]);
        //
        //         this.logger.LogInformation("Start login");
        //         var userData = await this.authService.LoginAsync(request, userAgentData);
        //
        //         this.logger.LogInformation("Add refresh token cookie");
        //         this.AddRefreshTokenCookie(new Token(), userData);
        //
        //         return this.Ok(userData);
        //     }
        //     catch (Exception e)
        //     {
        //         return this.BadRequest($"Ошибка при выходе в аккаунт:{e.Message}");
        //     }
        // }
        
        [AllowAnonymous]
        [HttpPost(nameof(SendCode))]
        public async Task<ActionResult> SendCode([FromBody] AuthPhoneDTO request)
        {
            try
            {
                smsService.SendSmsCodeAsync(request);
               return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest($"Ошибка при регистрации:{e.Message}");
            }
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Authorization))]
        public async Task<ActionResult> Authorization([FromBody] UserPhoneCodeDTO request)
        {
            try
            {
                if (smsService.ValidateCodeAsync(request).Result)
                {
                    var userAgentData = Utilities.GetUserAgentData(this.Request.Headers["User-Agent"]);
                    
                    this.logger.LogInformation("Start login");
                    var userData = await this.authService.RegistrationAndLoginByPhoneAsync(request, userAgentData);

                    this.logger.LogInformation("Add refresh token cookie");
                    this.AddRefreshTokenCookie(new Token(), userData);
                    return this.Ok(userData); 
                }
                return this.BadRequest($"Невірний код підтвердження");
            }
            catch (Exception e)
            {
                return this.BadRequest($"Ошибка при регистрации:{e.Message}");
            }
        }

        [Authorize]
        [HttpDelete(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var cookieExist = this.Request.Cookies.TryGetValue(this.refreshTokenKey, out var refreshTokenValue);
                if (!cookieExist)
                {
                    this.logger.LogError("Refresh token cookie is not found!");
                    throw new Exception("Куки рефреш токена отсутствуют!");
                }

                this.logger.LogInformation("Start logout");
                await this.authService.LogoutAsync(refreshTokenValue!);

                this.logger.LogInformation("Delete Refresh token cookie");
                this.Response.Cookies.Delete(this.refreshTokenKey);

                return this.Ok();
            }
            catch
            {
                return this.BadRequest("Ошибка при выходе из аккаунта!");
            }
        }

        [AllowAnonymous]
        [HttpPut(nameof(Refresh))]
        public async Task<ActionResult<UserData>> Refresh([FromBody] string accessToken)
        {
            try
            {
                var cookieIsExist = this.Request.Cookies.TryGetValue(this.refreshTokenKey, out var refreshTokenValue);
                if (!cookieIsExist)
                {
                    this.logger.LogError("Refresh token cookie is not found!");
                    throw new Exception("Куки рефреш токена отсутствуют!");
                }

                var userAgentData = Utilities.GetUserAgentData(this.Request.Headers["User-Agent"]);

                var tokens = new TokensData { AccessJwt = accessToken, RefreshJwt = refreshTokenValue! };

                this.logger.LogInformation("Start refresh");
                var userData = await this.tokensService.RefreshAsync(tokens, userAgentData);

                this.logger.LogInformation("Save refresh token");
                await this.tokensService.SaveRefreshTokenAsync(userData!.UserPhoneDto.Id, userData.TokensData.RefreshJwt, userAgentData);

                this.logger.LogInformation("Add refresh token cookie");
                this.AddRefreshTokenCookie(new Token(), userData);

                return this.Ok(userData);
            }
            catch
            {
                return this.Unauthorized("Невозможно авторизоваться!");
            }
        }

        private void AddRefreshTokenCookie(Token refreshToken, UserDataAlt userDataAlt)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = refreshToken.Expired,
                MaxAge = TimeSpan.FromMinutes(refreshToken.LifeTime)
            };

            this.Response.Cookies.Append(this.refreshTokenKey, userDataAlt.TokensData.RefreshJwt, cookieOptions);
        }
    }
}
