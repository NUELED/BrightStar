using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Jwt_Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly AppDbContext _db;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, AppDbContext db)
        {
            _jwtOptions = jwtOptions.Value;
            _db = db;   
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


    }
}
