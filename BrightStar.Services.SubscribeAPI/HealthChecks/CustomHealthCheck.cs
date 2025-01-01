using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BrightStar.Services.SubscribeAPI.HealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        private Random _random = new Random();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var responseTime = _random.Next(1, 300);

            return Task.FromResult(responseTime switch
            {
                < 100 => HealthCheckResult.Healthy("Healthy Result from CustomHealthCheck"),
               (> 100) and (< 200 )=> HealthCheckResult.Healthy("Degraded Result from CustomHealthCheck"),
               _=> HealthCheckResult.Unhealthy("Unhealthy Result from CustomHealthCheck")
            });
        }
    }
}
