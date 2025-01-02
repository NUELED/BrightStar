using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BrightStar.Services.Infrastructure.BackgroundJobs
{
    public class JobTestService : IJobTestService
    {
        private readonly ILogger<JobTestService> _logger;
        public JobTestService(ILogger<JobTestService> logger)
        {
          _logger = logger;
        }

        public void ContinuationJob()
        {
           _logger.LogInformation("Hello from a continuation job!");
        }

        public void DelayedJob()
        {
            _logger.LogInformation("Hello from a Delayed Job!");
        }

        public void FireAndForgetJob()
        {
            _logger.LogInformation("Hello from a FireAndForget job!");
        }

        public void RecurringJob()
        {
            _logger.LogInformation("Hello from a Recurring job!");
        }

    }
}
