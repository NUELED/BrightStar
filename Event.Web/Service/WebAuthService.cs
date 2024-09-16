using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Utility;
using Event.Web.Service.IService;

namespace Event.Web.Service
{
    public class WebAuthService : IWebAuthService
    {
        private readonly IBaseService _baseService;

        public WebAuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }


        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.EventAPIBase + "/api/auth/AssignRole"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.EventAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.EventAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
