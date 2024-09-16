using BrightStar.Services.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);


        // For Web
        //Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        //Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        //Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);


    }
}
