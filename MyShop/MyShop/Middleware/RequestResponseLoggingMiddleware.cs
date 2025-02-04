using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace MyShop.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log the incoming request
            await LogRequest(context);

            // Copy the response stream to allow logging
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                // Process the next middleware
                await _next(context);

                stopwatch.Stop();

                // Log the outgoing response
                await LogResponse(context, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                // Handle and log exceptions
                _logger.LogError(ex, "An error occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                // Restore the original response stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = context.Request;
            var builder = new StringBuilder();

            builder.AppendLine($"[Request] {request.Method} {request.Path}");

            if (request.QueryString.HasValue)
            {
                builder.AppendLine($"[QueryString] {request.QueryString.Value}");
            }

            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                builder.AppendLine($"[Body] {body}");
                request.Body.Position = 0;
            }

            _logger.LogInformation(builder.ToString());
        }

        private async Task LogResponse(HttpContext context, long elapsedMilliseconds)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                $"[Response] {context.Response.StatusCode} - {elapsedMilliseconds}ms\n{responseBody}");
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var result = $"{{\"error\":\"{ex.Message}\"}}";
            return context.Response.WriteAsync(result);
        }
    }
}
