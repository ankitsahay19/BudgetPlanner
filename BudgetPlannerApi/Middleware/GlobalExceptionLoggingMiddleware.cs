using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Bpst.API.Middleware
{
    public class GlobalExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionLoggingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionLoggingMiddleware(RequestDelegate next, ILogger<GlobalExceptionLoggingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            // Ensure a correlation id is present for tracking
            var correlationId = context.TraceIdentifier ?? Guid.NewGuid().ToString();
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log via ILogger (goes to configured sinks)
                _logger.LogError(ex, "Unhandled exception (CorrelationId={CorrelationId}) for request {Method} {Path}", correlationId, context.Request.Method, context.Request.Path);

                // Also write a compact JSON line to a local log file so operators can quickly inspect prod errors
                try
                {
                    var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    Directory.CreateDirectory(logDir);
                    var file = Path.Combine(logDir, $"errors-{DateTime.UtcNow:yyyy-MM-dd}.log");
                    var entry = new
                    {
                        TimeUtc = DateTime.UtcNow,
                        CorrelationId = correlationId,
                        Method = context.Request.Method,
                        Path = context.Request.Path.ToString(),
                        Message = ex.Message,
                        Exception = ex.ToString()
                    };
                    var json = JsonSerializer.Serialize(entry);
                    File.AppendAllText(file, json + Environment.NewLine);
                }
                catch
                {
                    // Swallow any logging-to-file failures to avoid masking the original error
                }

                // Return a safe ProblemDetails response to the client with the correlation id
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";

                    var problem = new
                    {
                        type = "https://httpstatuses.io/500",
                        title = "An unexpected error occurred.",
                        status = 500,
                        detail = "An internal server error occurred. Provide the X-Correlation-ID header to support.",
                        traceId = correlationId
                    };

                    await context.Response.WriteAsJsonAsync(problem);
                }
            }
        }
    }

    public static class GlobalExceptionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionLoggingMiddleware>();
        }
    }
}
