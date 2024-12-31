using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Jwt_Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, AppDbContext db, UserManager<AppUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _db = db;  
            _userManager = userManager; 
        }


        public string GenerateToken(AppUser appUser, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(_jwtOptions.Secret))
            {
                throw new ArgumentException("Secret key is missing.");
            }

            var tokenConfig = _db.TokenConfigs.FirstOrDefault();
            if (tokenConfig == null)
            {
                throw new Exception("Token configuration not found.");
            }

            
            var tokenExpiryInMinutes = tokenConfig.Time;
            var expirationTime = DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
               //new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expirationTime).ToUnixTimeSeconds().ToString() ),
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName.ToString()),
                
                //new Claim(JwtRegisteredClaimNames.Name, appUser.UserName.ToString())
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



        public async Task<TokenDto> GenerateToken2(AppUser appUser, IEnumerable<string> roles, bool populateExp)
        {
            if (string.IsNullOrEmpty(_jwtOptions.Secret))
            {
                throw new ArgumentException("Secret key is missing.");
            }

            var tokenConfig = await _db.TokenConfigs.FirstOrDefaultAsync();
            if (tokenConfig == null)
            {
                throw new Exception("Token configuration is missing in the database.");
            }

            var tokenExpiryInMinutes = tokenConfig.Time;
            var expirationTime = DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName.ToString())
            };

            if (roles != null)
            {
                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = GenerateRefreshToken();
            appUser.RefreshToken = refreshToken;

            if (populateExp)
            {
                appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            await _userManager.UpdateAsync(appUser);

            var accessToken = tokenHandler.WriteToken(token);

            return new TokenDto(accessToken, refreshToken);
        }



        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
           var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid token.");
            }
            //var user = await _userManager.FindByNameAsync(principal.Identity.Name);       
            var userid = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;     
            var user = await _userManager.FindByIdAsync(userid);            
            var roles = await _userManager.GetRolesAsync(user);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ArgumentException("Issues with refresh token.");
            }

           return  await GenerateToken2(user, roles, populateExp:false);   
        }

        private string GenerateRefreshToken()
        {
            var randomNumber =  new byte[32];   
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // Always validate the signing key           
                ValidateAudience = false, // Ignore audience validation for expired tokens
                ValidateLifetime = false, // Allow expired tokens for extraction
                ValidateIssuer = true, // Ensure the token's issuer matches the expected issuer
                ValidIssuer = _jwtOptions.Issuer, // Your application's issuer
                ValidAudience = _jwtOptions.Audience, // Expected audience
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }




    }
}
