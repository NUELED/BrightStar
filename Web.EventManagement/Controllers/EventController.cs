using BrightStar.Services.Application.BrightServices.Abstraction;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Web.EventManagement.SendEmail;

namespace Web.EventManagement.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService  _eventService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        public EventController(IEventService eventService, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _eventService = eventService;
            _userManager = userManager; 
            _emailSender = emailSender; 
        }


        public async Task<IActionResult> Index()
        {
            var events = _eventService.GetAllEvents();
            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
       // [Authorize(Roles =)]
        public async Task<IActionResult> Create(Event obj)
        {
            // Retrieve the UserId from the current user's claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Unable to determine the user ID.");
                return View(obj);
            }

            obj.UserId = userId;

            if (ModelState.IsValid)
            {
                _eventService.CreateEvent(obj);

                var emailRequest = new EmailDto
                {
                    To = User.Identity.Name, 
                    Subject = "Event Created Successfully",
                    Body = $"Dear {User.Identity.Name},<br/><br/>Your event titled '{obj.Title}' has been successfully created."
                };

                try
                {
                    await _emailSender.SendEmail2(emailRequest);
                    TempData["success"] = "The Event has been created successfully, and an email has been sent!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["success"] = "The Event has been created successfully, but there was an error sending the email.";
                    // Optionally log the error or handle it accordingly
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }

            }

            return View(obj);
        }

        public async Task<IActionResult> Update(int eventId)
		{
			try
			{
                var DataFromdb = _eventService.GetEventById(eventId);
                if (DataFromdb == null || eventId == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                if (DataFromdb == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                return View(DataFromdb);
            }
            catch (Exception ex)
            {
                
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Event obj)
        {
         
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Unable to determine the user ID.");
                return View(obj);
            }

            obj.UserId = userId;

            if (ModelState.IsValid && obj.EventId> 0)
            {
                _eventService.UpdateEvent(obj);

                var emailRequest = new EmailDto
                {
                    To = User.Identity.Name,
                    Subject = "Event Updated Successfully",
                    Body = $"Dear {User.Identity.Name},<br/><br/>Your event titled '{obj.Title}' has been successfully updated."
                };

                try
                {
                    await _emailSender.SendEmail2(emailRequest);
                    TempData["success"] = "The Event has been created successfully, and an email has been sent!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["success"] = "The Event has been created successfully, but there was an error sending the email.";
                   
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }

            }

            return View(obj);

        }

        public async Task<IActionResult> Delete(int eventId)
        {
            try
            {
                var DataFromdb = _eventService.GetEventById(eventId);
                if (eventId == null || eventId == 0)
                {
                    return RedirectToAction("Error", "Home");
                }

                if (DataFromdb is null)
                {
                    return RedirectToAction("Error", "Home");
                }
                return View(DataFromdb);
            }
            catch (Exception ex)
            {
                
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Event obj)
        {
            try
            {
                bool deleted = _eventService.DeleteEvent(obj.EventId);
                if (deleted)
                {
                    TempData["success"] = "The Event was successfully deleted.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Failed to delete the Event ";
                }
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int eventId)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.Identity.Name;

            
            var eventToSubscribe = _eventService.GetOnlyEventById(eventId);

            if (eventToSubscribe == null)
            {
                return NotFound();
            }


            
            var phoneNumber = await _userManager.GetPhoneNumberAsync(await _userManager.FindByIdAsync(userId));

            if (string.IsNullOrEmpty(phoneNumber))
            {
                
                TempData["error"] = "Phone number is required for subscription.";
                return RedirectToAction("EventDetails", new { id = eventId });
            }

            if (eventToSubscribe.CurrentParticipants >= eventToSubscribe.MaxParticipants)
            {
                TempData["error"] = "The event has reached the maximum number of participants.";
                return RedirectToAction("Index");
            }

          
            var subscription = new EventSubscription
            {
                SubscriptionId = Guid.NewGuid().ToString(),
                PhoneNumber = phoneNumber,
                UserId = userId,
                Username = username,
                EventId = eventId,
                SubscribedOn = DateTime.UtcNow
            };

            
             var result = await _eventService.CreateSubscriptionAsync(subscription);

             eventToSubscribe.CurrentParticipants++;
            _eventService.UpdateEvent(eventToSubscribe);

            if (result)
            {
                
                var emailRequest = new EmailDto
                {
                    To = User.Identity.Name,
                    Subject = "Event Created Successfully",
                    Body = $"Dear {User.Identity.Name},<br/><br/>You have successfully subscribed to the  event titled '{eventToSubscribe.Title}'. "
                };

                try
                {
                    await _emailSender.SendEmail2(emailRequest);
                    TempData["success"] = "You have successfully subscribed to the event, and an email has been sent!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["success"] = "The Event has been subscribed  successfully, but there was an error sending the email.";
                  
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
            else
            {
                TempData["error"] = "There was a problem subscribing to the event.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int eventId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Index");
            }

            var subscription = await _eventService.GetEventSubscriptionAsync(eventId, userId);
            if (subscription == null)
            {
                TempData["error"] = "Subscription not found.";
                return RedirectToAction("Index");
            }

            // Retrieve the event
            var eventDetails =  _eventService.GetEventById(eventId);
            if (eventDetails == null)
            {
                TempData["error"] = "Event not found.";
                return RedirectToAction("Index");
            }

            // Delete the subscription
            var result = await _eventService.DeleteEventSubscriptionAsync(subscription);
            if (result)
            {
                if (eventDetails.CurrentParticipants > 0)
                {
                    eventDetails.CurrentParticipants--;
                     _eventService.UpdateEvent(eventDetails);
                    var emailRequest = new EmailDto
                    {
                        To = User.Identity.Name,
                        Subject = "Event Created Successfully",
                        Body = $"Dear {User.Identity.Name},<br/><br/>You have successfully unsubscribed to the  event titled '{eventDetails.Title}'. "
                    };

                    try
                    {
                        await _emailSender.SendEmail2(emailRequest);
                        TempData["success"] = "You have successfully unsubscribed to the event, and an email has been sent!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "The Event has been unsubscribed successfully, but there was an error sending the email.";
                       
                        Console.WriteLine($"Error sending email: {ex.Message}");
                    }
                }
                else
                {
                    TempData["error"] = "Error occurred while unsubscribing. The event participant count is already at zero.";
                }
            }
            else
            {
                TempData["error"] = "Error occurred while unsubscribing.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MySubscriptions()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

           
            var userSubscriptions = await _eventService.GetUserSubscriptionsAsync(userId);

            if (userSubscriptions == null || !userSubscriptions.Any())
            {
                TempData["info"] = "You have no active subscriptions.";
                return View(new List<EventSubscription>());
            }


            return View(userSubscriptions);
        }




    }
}
