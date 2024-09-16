using BrightStar.Services.Application.BrightServices.Abstraction;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Utility;
using BrightStar.Services.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.EventManagement.SendEmail;

namespace Web.EventManagement.Controllers
{
    [Authorize(Roles =SD.RoleAdmin)]
    public class DashboardController : Controller
    {


        private readonly IEventService _eventService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        public DashboardController(IEventService eventService, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _eventService = eventService;
            _userManager = userManager;
            _emailSender = emailSender; 
        }

        public IActionResult Index()
        {
            var events = _eventService.GetAllEvents();
            return View(events);
        }



        public async Task<IActionResult> AllSubscriptions()
        {
            var villas = _eventService.GetAllSubscriptions();
            return View(villas);
        }

        [HttpPost]
        public async Task<IActionResult> AdminUnsubscribe(int eventId, string userId)
        {
            // Check if the current user is an admin
            if (!User.IsInRole(SD.RoleAdmin))
            {
                TempData["error"] = "You do not have permission to unsubscribe users from this event.";
                return RedirectToAction("Index");
            }

            // Check if the provided userId is valid
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Index");
            }

            // Get the event subscription for the specified user
            var subscription = await _eventService.GetEventSubscriptionAsync(eventId, userId);
            if (subscription == null)
            {
                TempData["error"] = "Subscription not found.";
                return RedirectToAction("Index");
            }

            // Retrieve the event details
            var eventDetails = _eventService.GetEventById(eventId);
            if (eventDetails == null)
            {
                TempData["error"] = "Event not found.";
                return RedirectToAction("Index");
            }

            // Delete the subscription
            var result = await _eventService.DeleteEventSubscriptionAsync(subscription);
            if (result)
            {
                // Update participant count
                if (eventDetails.CurrentParticipants > 0)
                {
                    eventDetails.CurrentParticipants--;
                    _eventService.UpdateEvent(eventDetails);

                    // Send email notification to the user
                    var emailRequest = new EmailDto
                    {
                        To = subscription.Username, // Assuming Username is the user's email
                        Subject = "Event Unsubscription",
                        Body = $"Dear {subscription.Username},<br/><br/>You have been unsubscribed from the event titled '{eventDetails.Title}' by the admin."
                    };

                    try
                    {
                       await _emailSender.SendEmail2(emailRequest);
                    
                        TempData["success"] = "User unsubscribed successfully and an email has been sent!";
                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "User unsubscribed successfully, but there was an error sending the email.";
                        Console.WriteLine($"Error sending email: {ex.Message}");
                    }
                }
                else
                {
                    TempData["error"] = "The event participant count is already at zero.";
                }
            }
            else
            {
                TempData["error"] = "Error occurred while unsubscribing the user.";
            }

            return RedirectToAction("Index");
        }



    }
}
