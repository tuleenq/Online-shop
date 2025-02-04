using ATS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType == "application/json" && context.Request.Method == "POST")
        {
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // Reset the stream position to allow the next middleware to read it
            context.Request.Body.Position = 0;

            if (!string.IsNullOrEmpty(requestBody))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                try
                {
                    var model = JsonSerializer.Deserialize<ResumeViewModel>(requestBody, options);

                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(model, null, null);
                    bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

                    if (!isValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var errors = validationResults.Select(v => v.ErrorMessage);
                        var errorResponse = JsonSerializer.Serialize(new { errors });
                        await context.Response.WriteAsync(errorResponse);
                        return;
                    }
                }
                catch (JsonException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid JSON payload");
                    return;
                }
            }
        }

        await _next(context);
    }
}
