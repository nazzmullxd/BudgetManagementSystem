using Business.Models;
using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<UserService> logger,
            IJwtService jwtService)
            : base(userRepository, auditService)
        {
            _logger = logger;
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public void Register(User user, string password)
        {
            _logger.LogInformation("Registering user with email {Email}", user?.Email);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            // Validate password strength
            if (!IsValidPassword(password))
                throw new ArgumentException("Password does not meet security requirements.", nameof(password));

            // Check if user already exists
            var existingUser = _userRepository.GetByEmail(user.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed for email {Email}: User already exists", user.Email);
                throw new ArgumentException("A user with this email already exists.");
            }

            // Hash password securely with BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

            _userRepository.Add(user);
            LogAction(_logger, user.UserId, "Register", $"User registered with email {user.Email}");
            _logger.LogInformation("User registered with ID {UserId}", user.UserId);
        }

        public Task<LoginResponse> LoginAsync(string email, string password)
        {
            _logger.LogInformation("Login attempt for email {Email}", email);

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Login failed: Email is required");
                throw new ArgumentException("Email is required.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("Login failed: Password is required");
                throw new ArgumentException("Password is required.", nameof(password));
            }

            var user = _userRepository.GetByEmail(email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for email {Email}: Invalid credentials", email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate JWT tokens
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token (in a real application, you'd store this in the database)
            // For now, we'll implement a simple in-memory storage or return both tokens
            var accessTokenExpirationMinutes = _jwtService.GetAccessTokenExpirationMinutes();
            
            var loginResponse = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                Username = user.Name, // Using the computed Name property
                Email = user.Email ?? string.Empty,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7) // Default 7 days for refresh token
            };

            LogAction(_logger, user.UserId, "Login", $"User logged in with email {email}");
            _logger.LogInformation("User {UserId} logged in successfully", user.UserId);
            
            return Task.FromResult(loginResponse);
        }

        public Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt");

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogError("Refresh token failed: Token is required");
                throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
            }

            if (!_jwtService.ValidateRefreshToken(refreshToken))
            {
                _logger.LogWarning("Refresh token failed: Invalid token");
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            // In a real application, you would:
            // 1. Look up the refresh token in the database
            // 2. Verify it's not expired or revoked
            // 3. Get the associated user
            // For now, we'll throw an exception as this needs database implementation
            throw new NotImplementedException("Refresh token functionality requires database implementation for token storage.");
        }

        public Task RevokeRefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Revoke refresh token attempt");

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogError("Revoke refresh token failed: Token is required");
                throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
            }

            // In a real application, you would mark the refresh token as revoked in the database
            throw new NotImplementedException("Revoke refresh token functionality requires database implementation for token storage.");
        }

        public User? GetUserById(string userId)
        {
            _logger.LogInformation("Retrieving user with ID {UserId}", userId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("GetUserById failed: User ID is required");
                throw new ArgumentException("User ID is required.", nameof(userId));
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
            }
            else
            {
                LogAction(_logger, userId, "GetUserById", $"Retrieved user with ID {userId}");
                _logger.LogInformation("Retrieved user with ID {UserId}", userId);
            }
            return user;
        }

        public void UpdateUser(User user)
        {
            _logger.LogInformation("Updating user with ID {UserId}", user?.UserId);

            if (user == null)
            {
                _logger.LogError("UpdateUser failed: User cannot be null");
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = _userRepository.GetById(user.UserId);
            if (existingUser == null)
            {
                _logger.LogError("UpdateUser failed: User with ID {UserId} not found", user.UserId);
                throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
            }

            _userRepository.Update(user);
            LogAction(_logger, user.UserId, "UpdateUser", $"User updated with ID {user.UserId}");
            _logger.LogInformation("User with ID {UserId} updated", user.UserId);
        }

        public void DeleteUser(string userId)
        {
            _logger.LogInformation("Deleting user with ID {UserId}", userId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("DeleteUser failed: User ID is required");
                throw new ArgumentException("User ID is required.", nameof(userId));
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError("DeleteUser failed: User with ID {UserId} not found", userId);
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            _userRepository.Delete(user);
            LogAction(_logger, userId, "DeleteUser", $"User deleted with ID {userId}");
            _logger.LogInformation("User with ID {UserId} deleted", userId);
        }

        public void ChangePassword(string userId, string currentPassword, string newPassword)
        {
            _logger.LogInformation("Password change request for user {UserId}", userId);

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentException("Current password is required.", nameof(currentPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required.", nameof(newPassword));

            // Validate new password strength
            if (!IsValidPassword(newPassword))
                throw new ArgumentException("New password does not meet security requirements.", nameof(newPassword));

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError("ChangePassword failed: User with ID {UserId} not found", userId);
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                _logger.LogWarning("Password change failed for user {UserId}: Invalid current password", userId);
                throw new UnauthorizedAccessException("Current password is incorrect.");
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            LogAction(_logger, userId, "ChangePassword", "Password changed successfully");
            _logger.LogInformation("Password changed for user {UserId}", userId);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Password must be at least 8 characters long
            if (password.Length < 8)
                return false;

            // Must contain at least one uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Must contain at least one lowercase letter
            if (!password.Any(char.IsLower))
                return false;

            // Must contain at least one digit
            if (!password.Any(char.IsDigit))
                return false;

            // Must contain at least one special character
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }

        public bool ValidatePasswordStrength(string password, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                validationErrors.Add("Password is required.");
                return false;
            }

            if (password.Length < 8)
                validationErrors.Add("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                validationErrors.Add("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                validationErrors.Add("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                validationErrors.Add("Password must contain at least one number.");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                validationErrors.Add("Password must contain at least one special character.");

            // Check for common weak passwords
            var weakPasswords = new[] { "password", "123456", "qwerty", "abc123", "password123" };
            if (weakPasswords.Any(weak => password.ToLower().Contains(weak)))
                validationErrors.Add("Password contains common weak patterns.");

            return validationErrors.Count == 0;
        }
    }
}