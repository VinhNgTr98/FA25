using System.Text.Json;

namespace CartManagement_Api.Services
{
    /// <summary>
    /// Middleware chuyển mọi exception sang ProblemDetails JSON.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await Write(ctx, 409, "Concurrency conflict", ex.Message, "https://example.com/errors/concurrency");
            }
            catch (ArgumentException ex)
            {
                await Write(ctx, 400, "Bad Request", ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await Write(ctx, 404, "Not Found", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                await Write(ctx, 500, "Server Error", "Unexpected error");
            }
        }

        private static async Task Write(HttpContext ctx, int status, string title, string detail, string? type = null)
        {
            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";
            var payload = new
            {
                type = type ?? "about:blank",
                title,
                status,
                detail,
                traceId = ctx.TraceIdentifier
            };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }

    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}