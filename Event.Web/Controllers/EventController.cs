using BrightStar.Services.Application.Common.DTO;
using Event.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Event.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }


        public async Task<IActionResult> EventIndex()
        {
            List<EventDto>? list = new();

            ResponseDto response = await _eventService.GetAllEventAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<EventDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(list);
        }


        public async Task<IActionResult> EventCreate()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EventCreate(EventDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _eventService.CreateEventAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Event created successfully.";
                    return RedirectToAction(nameof(EventIndex));
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EventUpdate(int eventId)
        {
            ResponseDto response = await _eventService.GetEventByIdAsync(eventId);

            if (response != null && response.IsSuccess)
            {
                EventDto model = JsonConvert.DeserializeObject<EventDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }


        //[HttpPost]
        //public async Task<IActionResult> EventUpdate(EventDto eventDto)
        //{
        //    ResponseDto response = await _eventService.UpdateEventAsync(eventDto);

        //    if (response != null && response.IsSuccess)
        //    {
        //        EventDto model = JsonConvert.DeserializeObject<EventDto>(Convert.ToString(response.Result));
        //        return View(model);
        //    }

        //    return NotFound();
        //}

        [HttpPost]
        public async Task<IActionResult> EventUpdate(EventDto eventDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _eventService.UpdateEventAsync(eventDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Event updated successfully.";
                    return RedirectToAction(nameof(EventIndex));
                }
            }
            return View(eventDto);
        }



        [HttpGet]
        public async Task<IActionResult> EventDelete(int eventId)
        {
            ResponseDto response = await _eventService.GetEventByIdAsync(eventId);

            if (response != null && response.IsSuccess)
            {
                EventDto model = JsonConvert.DeserializeObject<EventDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }






        [HttpPost]
        public async Task<IActionResult> EventDelete(EventDto eventDto)
        {
            ResponseDto response = await _eventService.DeleteEventAsync(eventDto.EventId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully.";
                return RedirectToAction(nameof(EventIndex));
            }

            return View(eventDto);
        }
    }
}
