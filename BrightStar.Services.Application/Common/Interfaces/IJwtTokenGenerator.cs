using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser appUser, IEnumerable<string> roles);
        Task<TokenDto> GenerateToken2(AppUser appUser, IEnumerable<string> roles, bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
