using BrightStar.Services.Domain.Entities;
using FakeItEasy;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.EventManagement.Controllers;
using Web.EventManagement.ViewModels;

namespace UnitTests.BrightStar
{
 
    public class AccountControllerTest
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            
            _userManager = A.Fake<UserManager<AppUser>>(x => x.WithArgumentsForConstructor(
                new object[] { A.Fake<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null }));

            _signInManager = A.Fake<SignInManager<AppUser>>(x => x.WithArgumentsForConstructor(
                new object[] { _userManager, A.Fake<IHttpContextAccessor>(), A.Fake<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null }));

            _roleManager = A.Fake<RoleManager<IdentityRole>>(x => x.WithArgumentsForConstructor(
                new object[] { A.Fake<IRoleStore<IdentityRole>>(), null, null, null, null }));

            
            var fakeRoles = new List<IdentityRole>
             {
              new IdentityRole { Name = "Admin" },
              new IdentityRole { Name = "User" }
             }.AsQueryable();

            A.CallTo(() => _roleManager.Roles).Returns(fakeRoles);

            
            _controller = new AccountController(_userManager, _signInManager, _roleManager);
        }

        [Fact]
        public async Task AccountController_Register_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var registerVM = new RegisterVM();
            _controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _controller.Register(registerVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RegisterVM>(viewResult.Model);
        }

        [Fact]
        public async Task AccountController_Register_ReturnsView_WhenRegistrationFails()
        {
            // Arrange
            var registerVM = new RegisterVM
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Admin"
            };

            // Mock a failed registration
            A.CallTo(() => _userManager.CreateAsync(A<AppUser>.Ignored, A<string>.Ignored))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "Registration failed." }));

            // Act
            var result = await _controller.Register(registerVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RegisterVM>(viewResult.Model);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }



    }






}
