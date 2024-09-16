using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Application.Common.Utility;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Jwt_Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGen;
        //private readonly IBaseService _baseService;

        public AuthService(AppDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGen /*IBaseService baseService*/)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGen = jwtTokenGen;
            //_baseService = baseService;
        }



        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.AppUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }
        public  async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.service_id.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = string.Empty, };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGen.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token,
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            AppUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var UserToReturn = _db.AppUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = UserToReturn.Email,
                        Id = UserToReturn.Id,
                        Name = UserToReturn.Name,
                        PhoneNumber = UserToReturn.PhoneNumber,
                    };

                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {


            }

            return "Error Encountered";
        }




    





    }
}
