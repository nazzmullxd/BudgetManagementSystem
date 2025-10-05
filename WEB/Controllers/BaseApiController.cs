using Microsoft.AspNetCore.Mvc;
using WEB.Models;

namespace WEB.Controllers
{
    /// <summary>
    /// Base controller with common functionality for all API controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Validates model state and returns a validation error response if invalid
        /// </summary>
        /// <returns>BadRequestObjectResult if model state is invalid, null if valid</returns>
        protected IActionResult? ValidateModelState()
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    );

                var validationResponse = new ValidationErrorResponse
                {
                    Status = 400,
                    TraceId = HttpContext.TraceIdentifier,
                    Path = HttpContext.Request.Path.Value,
                    ValidationErrors = validationErrors
                };

                return BadRequest(validationResponse);
            }

            return null;
        }

        /// <summary>
        /// Gets the current authenticated user ID from the JWT token claims
        /// </summary>
        /// <returns>User ID if authenticated, null otherwise</returns>
        protected string? GetCurrentUserId()
        {
            return User.FindFirst("user_id")?.Value ?? User.FindFirst("UserId")?.Value;
        }

        /// <summary>
        /// Gets the current authenticated user email from the JWT token claims
        /// </summary>
        /// <returns>User email if authenticated, null otherwise</returns>
        protected string? GetCurrentUserEmail()
        {
            return User.FindFirst("email")?.Value;
        }

        /// <summary>
        /// Checks if the current user is authenticated
        /// </summary>
        /// <returns>True if authenticated, false otherwise</returns>
        protected bool IsAuthenticated()
        {
            return User.Identity?.IsAuthenticated == true;
        }

        /// <summary>
        /// Returns an unauthorized result if the user is not authenticated
        /// </summary>
        /// <returns>UnauthorizedResult if not authenticated, null if authenticated</returns>
        protected IActionResult? RequireAuthentication()
        {
            if (!IsAuthenticated())
            {
                return Unauthorized(new ErrorResponse
                {
                    Status = 401,
                    Error = "Unauthorized",
                    Message = "Authentication is required to access this resource.",
                    TraceId = HttpContext.TraceIdentifier,
                    Path = HttpContext.Request.Path.Value
                });
            }

            return null;
        }
    }
}