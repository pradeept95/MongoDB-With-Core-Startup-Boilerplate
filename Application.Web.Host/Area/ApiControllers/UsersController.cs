using Api.Helper.ContentWrapper.Core.BaseApiController;
using Application.Core.Configuration.TokenAuth;
using Application.Core.Constants; 
using Application.Core.Dto.AppUser;
using Application.Core.Dto.Authenticate;
using Application.Core.Models;
using Application.Services.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.ResultWrapper.AspNetCore.ResponseModel;
using Api.ResultWrapper.AspNetCore.WrapperModel;
using AspNetCore.Security.PasswordHasher;

namespace Application.Area.ApiControllers
{
    public class UsersController : BaseApiController
    {
        private IAppUsersService _userService;
        private readonly IOptions<Settings> _settings;
        private readonly IOptions<TokenAuthConfiguration> _configuration;

        public UsersController(IOptions<Settings> settings, IAppUsersService userService, IOptions<TokenAuthConfiguration> configuration)
        {
            _userService = userService;
            _settings = settings;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ResultDto<LoginResponseDto>> Authenticate([FromBody]LoginRequestDto input)
        {
             
            var result = new LoginResponseDto
            {
                IsLoginSuccess = false,
                Email = input.UsernameOrEmail,
                Token = string.Empty,
                UserId = string.Empty
            };

            var user = await _userService.Authenticate(input);

            if (user == null)
                return new ResultDto<LoginResponseDto>(result);

            var userTokenExpiryDays = 1;
            if (input.IsRemember) userTokenExpiryDays = 7;

            var Identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                     new Claim(ClaimTypes.Name, user.UserName.ToString()),
                      new Claim(ClaimTypes.Email, user.Email.ToString())
                });

            var accessToken = CreateAccessToken(CreateJwtClaims(Identity), TimeSpan.FromDays(userTokenExpiryDays));

            var loginResult = new LoginResponseDto
            {
                Token = accessToken,
                EncryptedToken = GetEncrpyedAccessToken(accessToken),
                Expires = userTokenExpiryDays * 24 * 60,
                UserId = user.Id.ToString(),
                FullName = string.IsNullOrEmpty(user.MiddleName) ? user.FirstName + " " + user.LastName : user.FirstName + " " + user.MiddleName + " " + user.LastName,
                Email = user.Email,
                IsLoginSuccess = true
            };

            return new ResultDto<LoginResponseDto>(loginResult);
        }

        [HttpGet("GetLogedInUser")]
        public async Task<ResultDto<AppUsersDto>> Get()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId == null)
            {
                throw new ApiException("User not loged in.", 401);
            }

            var result = await _userService.Get(userId) ?? new AppUsersDto();
            return new ResultDto<AppUsersDto>(result);
        }


        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow; 
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Value.Expiration),
                signingCredentials: _configuration.Value.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}
