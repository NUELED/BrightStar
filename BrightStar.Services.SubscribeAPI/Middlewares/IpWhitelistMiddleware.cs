namespace BrightStar.Services.SubscribeAPI.Middlewares
{
    public class IpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _whitelistedIps;

        public IpWhitelistMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            // Fetch the list of allowed IPs from configuration
            _whitelistedIps = configuration.GetSection("IpWhitelist").Get<List<string>>() ?? new List<string>();
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (!_whitelistedIps.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Your IP is not allowed.");
                return;
            }

            await _next(context);
        }
    }

}
