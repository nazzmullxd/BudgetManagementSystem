using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Services;
using Database.Model;
using WEB.Models;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = _userService.Login(request.Email, request.Password);
                var user = _userService.GetUserById(userId);

                if (user == null)
                    return Unauthorized("Invalid credentials");

                var token = GenerateJwtToken(user);
                var response = new AuthResponse
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email {Email}", request.Email);
                return StatusCode(500, "An error occurred during login");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PreferredCurrencyId = request.PreferredCurrencyId
                };

                _userService.Register(user, request.Password);

                var token = GenerateJwtToken(user);
                var response = new AuthResponse
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email {Email}", request.Email);
                return StatusCode(500, "An error occurred during registration");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid token");

                var user = _userService.GetUserById(userId);
                if (user == null)
                    return Unauthorized("User not found");

                var token = GenerateJwtToken(user);
                var response = new AuthResponse
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, "An error occurred during token refresh");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "MyVeryLongSecretKeyForJWTTokenGeneration123456789";
            var issuer = jwtSettings["Issuer"] ?? "BudgetManagementSystem";
            var audience = jwtSettings["Audience"] ?? "BudgetManagementSystemUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.UserId),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}