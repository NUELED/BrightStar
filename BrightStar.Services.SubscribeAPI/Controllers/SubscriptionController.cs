using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightStar.Services.SubscribeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequestDto request)
        {
            var result = await _subscriptionService.SubscribeAsync(request.PhoneNumber, request.service_id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> CheckSubscriptionStatus([FromQuery] string phoneNumber, string username)
        {
            var result = await _subscriptionService.CheckSubscriptionStatusAsync(phoneNumber, username);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeRequestDto request)
        {
            var result = await _subscriptionService.UnsubscribeAsync(request.PhoneNumber, request.service_id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
