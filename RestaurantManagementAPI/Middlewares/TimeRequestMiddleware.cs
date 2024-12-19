using System.Diagnostics;

namespace RestaurantManagementAPI.Middlewares
{
    public class TimeRequestMiddleware : IMiddleware
    {
        private readonly Stopwatch stopwatch;
        private readonly ILogger<TimeRequestMiddleware> _logger;
        public TimeRequestMiddleware(ILogger<TimeRequestMiddleware> logger)
        {
            stopwatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            if (time / 1000 > 4)
                _logger.LogInformation($"For request {context.Request.Method} named {context.Request.Path} took {time} ms");
        }
    }
}
