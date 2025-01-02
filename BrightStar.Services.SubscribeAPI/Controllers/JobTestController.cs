using BrightStar.Services.Application.Common.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightStar.Services.SubscribeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestService _testService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobTestController(IJobTestService testService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _testService = testService;    
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet("FireAndForgetJob")]
        public async Task<IActionResult> CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _testService.FireAndForgetJob());

            return Ok();
        }

        [HttpGet("DelayedJob")]
        public async Task<IActionResult> CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _testService.DelayedJob(), TimeSpan.FromSeconds(60) );

            return Ok();
        }

        [HttpGet("RecuringJob")]
        public async Task<IActionResult> CreateRecuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _testService.RecurringJob(), Cron.Daily);

            return Ok();
        }

        [HttpGet("ContinuationJob")]
        public async Task<IActionResult> CreateContinuationJob()
        {
            var parentjobId = _backgroundJobClient.Enqueue(() => _testService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentjobId, () => _testService.ContinuationJob());

            return Ok();
        }




    }
}
