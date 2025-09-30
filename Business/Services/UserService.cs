using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<UserService> logger)
            : base(userRepository, auditService)
        {
            _logger = logger;
        }

        public void Register(User user, string password)
        {
            _logger.LogInformation("Registering user with email {Email}", user?.Email);

            if (user == null)
            {
                _logger.LogError("Register failed: User cannot be null");
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("Register failed: Password is required");
                throw new ArgumentException("Password is required.", nameof(password));
            }

            // Hash password (simplified - in production use proper password hashing)
            user.PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            _userRepository.Add(user);
            LogAction(_logger, user.UserId, "Register", $"User registered with email {user.Email}");
            _logger.LogInformation("User registered with ID {UserId}", user.UserId);
        }

        public string Login(string email, string password)
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

            // Find user by email - simplified approach for now
            var user = _userRepository.GetByEmail(email);
            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            if (user == null || user.PasswordHash != hashedPassword)
            {
                _logger.LogWarning("Login failed for email {Email}: Invalid credentials", email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            LogAction(_logger, user.UserId, "Login", $"User logged in with email {email}");
            _logger.LogInformation("User {UserId} logged in successfully", user.UserId);
            
            // Return user ID as login token (simplified - in production use JWT or proper tokens)
            return user.UserId;
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
    }
}