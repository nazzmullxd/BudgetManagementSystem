using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WEB.Models;

namespace WEB.Middleware
{
    /// <summary>
    /// Global exception handling middleware that provides centralized error handling
    /// and consistent error responses across the application
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Invokes the middleware to handle the HTTP request
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions and generates appropriate error responses
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception with appropriate level and context
            LogException(context, exception);

            // Ensure response hasn't been started
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Cannot write error response. Response has already started.");
                return;
            }

            // Create error response
            var errorResponse = CreateErrorResponse(context, exception);

            // Set response content type and status
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.Status;

            // Serialize and write response
            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// Creates an appropriate error response based on the exception type
        /// </summary>
        private ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
        {
            var errorResponse = exception switch
            {
                // Validation and argument errors
                ArgumentException or ArgumentNullException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Error = "BadRequest",
                    Message = "Invalid request parameters.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Authentication and authorization errors
                UnauthorizedAccessException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Error = "Unauthorized",
                    Message = "Authentication is required to access this resource.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Resource not found errors
                KeyNotFoundException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Error = "NotFound",
                    Message = "The requested resource was not found.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Invalid operation errors
                InvalidOperationException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Error = "InvalidOperation",
                    Message = "The requested operation is not valid in the current state.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Not implemented errors
                NotImplementedException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.NotImplemented,
                    Error = "NotImplemented",
                    Message = "This feature is not yet implemented.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Timeout errors
                TimeoutException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.RequestTimeout,
                    Error = "Timeout",
                    Message = "The request timed out. Please try again.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Validation errors from ASP.NET Core model validation
                BadHttpRequestException => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Error = "BadRequest",
                    Message = "The request is malformed or contains invalid data.",
                    Details = _environment.IsDevelopment() ? exception.Message : null
                },

                // Default case for all other exceptions
                _ => new ErrorResponse
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Error = "InternalServerError",
                    Message = "An internal server error occurred. Please try again later.",
                    Details = _environment.IsDevelopment() ? exception.ToString() : null
                }
            };

            // Set common properties
            errorResponse.TraceId = context.TraceIdentifier;
            errorResponse.Path = context.Request.Path.Value;
            errorResponse.Timestamp = DateTime.UtcNow;

            return errorResponse;
        }

        /// <summary>
        /// Logs the exception with appropriate context and severity
        /// </summary>
        private void LogException(HttpContext context, Exception exception)
        {
            var logLevel = GetLogLevel(exception);
            var userId = context.User?.FindFirst("user_id")?.Value ?? "Anonymous";
            var requestPath = context.Request.Path.Value;
            var requestMethod = context.Request.Method;
            var traceId = context.TraceIdentifier;

            // Create structured log data
            using var scope = _logger.BeginScope(new Dictionary<string, object?>
            {
                ["UserId"] = userId,
                ["TraceId"] = traceId,
                ["RequestPath"] = requestPath,
                ["RequestMethod"] = requestMethod,
                ["ExceptionType"] = exception.GetType().Name
            });

            // Log with appropriate level
            switch (logLevel)
            {
                case LogLevel.Warning:
                    _logger.LogWarning(exception, 
                        "Client error occurred for {RequestMethod} {RequestPath} by user {UserId}. " +
                        "TraceId: {TraceId}", requestMethod, requestPath, userId, traceId);
                    break;

                case LogLevel.Error:
                    _logger.LogError(exception, 
                        "Server error occurred for {RequestMethod} {RequestPath} by user {UserId}. " +
                        "TraceId: {TraceId}", requestMethod, requestPath, userId, traceId);
                    break;

                case LogLevel.Critical:
                    _logger.LogCritical(exception, 
                        "Critical error occurred for {RequestMethod} {RequestPath} by user {UserId}. " +
                        "TraceId: {TraceId}", requestMethod, requestPath, userId, traceId);
                    break;

                default:
                    _logger.LogInformation(exception, 
                        "Exception occurred for {RequestMethod} {RequestPath} by user {UserId}. " +
                        "TraceId: {TraceId}", requestMethod, requestPath, userId, traceId);
                    break;
            }
        }

        /// <summary>
        /// Determines the appropriate log level based on exception type
        /// </summary>
        private static LogLevel GetLogLevel(Exception exception)
        {
            return exception switch
            {
                ArgumentException or ArgumentNullException or InvalidOperationException or KeyNotFoundException => LogLevel.Warning,
                UnauthorizedAccessException => LogLevel.Warning,
                NotImplementedException => LogLevel.Information,
                TimeoutException => LogLevel.Warning,
                BadHttpRequestException => LogLevel.Warning,
                _ => LogLevel.Error
            };
        }
    }

    /// <summary>
    /// Extension methods for registering the global exception middleware
    /// </summary>
    public static class GlobalExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Registers the global exception middleware in the pipeline
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}