using ATS_CV.Resourses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IStringLocalizer<Resource> _localize;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IStringLocalizer<Resource> localize)
    {
        _logger = logger;
        _localize = localize;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        
        _logger.LogError(exception, "Unhandled exception occurred.");

        httpContext.Response.ContentType = "application/json";

#if PRODUCTION
        var (statusCode, message) = exception switch
        {
            InvalidOperationException => ((int)HttpStatusCode.NotFound, exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };
        try
        {
            if (exception.Message.StartsWith("tran:"))
            {
                _logger.LogError(exception, "Unhandled exception occurred.");

                var culture = httpContext.Request.Headers["Accept-Language"].ToString();
                if (string.IsNullOrEmpty(culture))
                {
                    culture = "ar";
                }


                var cultureInfo = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;

                var key = exception.Message.Split(':')[1];
                var localizedMessage = _localize[key].Value;

                if (localizedMessage == null)
                {
                    localizedMessage = new LocalizedString(key, $"Message key '{key}' not found.");
                }

                (statusCode, message) = ((int)HttpStatusCode.BadRequest, localizedMessage);

            }

            else
            {
                (statusCode, message) = ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }


        }
        catch(Exception ex) 
        {
            throw;
        }


#else

        var (statusCode, message) = exception switch
        {
            InvalidOperationException => ((int)HttpStatusCode.NotFound, exception.Message),
            _ => (httpContext.Response.StatusCode, exception.Message)
        };
#endif
       
        httpContext.Response.StatusCode = 200;
        var errorResponse = new
        {
            StatusCode = statusCode,
            Message = message
        };
        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true 
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions), cancellationToken);

        return true;
    }
}
