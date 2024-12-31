using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.DTO
{
    public record TokenDto(string AccessToken, string RefreshToken);
 
}
