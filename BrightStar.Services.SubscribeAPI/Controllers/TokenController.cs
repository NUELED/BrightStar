using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Infrastructure.Jwt_Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightStar.Services.SubscribeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtService;
        protected ResponseDto _response;

        public TokenController(IJwtTokenGenerator jwtService)
        {
            _jwtService = jwtService;
            _response = new();
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto model)
        {
            var returnedTokenDto = await _jwtService.RefreshToken(model);
            if (returnedTokenDto.AccessToken == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Details incorrect";
                return BadRequest(_response);
            }
            _response.Result = returnedTokenDto;
            return Ok(_response);
        }


        








    }
}
