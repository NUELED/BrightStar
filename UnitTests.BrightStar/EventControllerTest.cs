using BrightStar.Services.Application.BrightServices.Abstraction;
using BrightStar.Services.Domain.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.EventManagement.Controllers;
using Web.EventManagement.SendEmail;

namespace UnitTests.BrightStar
{
    public class EventControllerTest
    {
        private readonly EventController _controller;
        private readonly IEventService _eventService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpContext _httpContext;

        public EventControllerTest()
        {
            // Initialize mocks
            _eventService = A.Fake<IEventService>();
            _emailSender = A.Fake<IEmailSender>();
            _userManager = A.Fake<UserManager<AppUser>>();

            // Setup controller with mocks
            _controller = new EventController(_eventService, _userManager, _emailSender);

            // Set up HttpContext and User Claims
            _httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "testuser@example.com")
            }, "mock"));

            _httpContext.User = user;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };
        }

        [Fact]
        public async Task Create_ReturnsRedirectToIndex_WhenEventCreatedSuccessfully()
        {
            // Arrange
            var eventObj = new Event { Title = "Test Event", MaxParticipants = 100, CurrentParticipants = 0 };
            A.CallTo(() => _eventService.CreateEvent(eventObj)).DoesNothing();

            // Act
            var result = await _controller.Create(eventObj);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Subscribe_ReturnsRedirectToIndex_WhenSubscribedSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var eventToSubscribe = new Event { EventId = eventId, Title = "Test Event", CurrentParticipants = 0, MaxParticipants = 10 };

            // Mock _eventService
            A.CallTo(() => _eventService.GetOnlyEventById(eventId)).Returns(eventToSubscribe);
            A.CallTo(() => _eventService.CreateSubscriptionAsync(A<EventSubscription>._)).Returns(true);
            A.CallTo(() => _eventService.UpdateEvent(eventToSubscribe)).DoesNothing();

            // Mock UserManager to return a valid phone number
            var appUser = new AppUser { Id = "123", UserName = "testuser@example.com" };
            A.CallTo(() => _userManager.FindByIdAsync("123")).Returns(Task.FromResult(appUser));
            A.CallTo(() => _userManager.GetPhoneNumberAsync(appUser)).Returns(Task.FromResult("1234567890"));

            // Act
            var result = await _controller.Subscribe(eventId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }




}
