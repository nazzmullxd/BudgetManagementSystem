using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Services;
using Database.Model;
using WEB.Models;
using WEB.Models.Requests;

namespace WEB.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IJwtService jwtService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate model state
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            var loginResponse = await _userService.LoginAsync(request.Email, request.Password);
            
            var response = new AuthResponse
            {
                Token = loginResponse.AccessToken,
                RefreshToken = loginResponse.RefreshToken,
                UserId = loginResponse.UserId,
                Email = loginResponse.Email,
                Name = loginResponse.Username,
                ExpiresAt = loginResponse.AccessTokenExpiration
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Validate model state
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            // Validate password strength
            if (!_userService.ValidatePasswordStrength(request.Password, out var validationErrors))
            {
                return BadRequest(new ValidationErrorResponse
                {
                    Status = 400,
                    TraceId = HttpContext.TraceIdentifier,
                    Path = HttpContext.Request.Path.Value,
                    ValidationErrors = new Dictionary<string, string[]> { ["Password"] = validationErrors.ToArray() }
                });
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PreferredCurrencyId = request.PreferredCurrencyId
            };

            _userService.Register(user, request.Password);

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var accessTokenExpirationMinutes = _jwtService.GetAccessTokenExpirationMinutes();
            
            var response = new AuthResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes)
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate model state
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            var loginResponse = await _userService.RefreshTokenAsync(request.RefreshToken);
            
            var response = new AuthResponse
            {
                Token = loginResponse.AccessToken,
                RefreshToken = loginResponse.RefreshToken,
                UserId = loginResponse.UserId,
                Email = loginResponse.Email,
                Name = loginResponse.Username,
                ExpiresAt = loginResponse.AccessTokenExpiration
            };

            return Ok(response);
        }



        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // Validate model state
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            // Require authentication
            var authResult = RequireAuthentication();
            if (authResult != null)
                return authResult;

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token claims");

            // Validate new password strength
            if (!_userService.ValidatePasswordStrength(request.NewPassword, out var validationErrors))
            {
                return BadRequest(new ValidationErrorResponse
                {
                    Status = 400,
                    TraceId = HttpContext.TraceIdentifier,
                    Path = HttpContext.Request.Path.Value,
                    ValidationErrors = new Dictionary<string, string[]> { ["NewPassword"] = validationErrors.ToArray() }
                });
            }

            _userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPost("validate-password")]
        public IActionResult ValidatePassword([FromBody] ValidatePasswordRequest request)
        {
            // Validate model state
            var validationResult = ValidateModelState();
            if (validationResult != null)
                return validationResult;

            var isValid = _userService.ValidatePasswordStrength(request.Password, out var validationErrors);

            return Ok(new 
            { 
                isValid = isValid, 
                errors = validationErrors,
                requirements = new
                {
                    minLength = 8,
                    requireUppercase = true,
                    requireLowercase = true,
                    requireNumber = true,
                    requireSpecialCharacter = true
                }
            });
        }
    }
}