using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<UserService> logger)
            : base(userRepository)
        {
            _userRepository = userRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateUser(User user)
        {
            _logger.LogInformation("Creating user with ID {UserId}", user?.UserId);

            if (user == null)
            {
                _logger.LogError("CreateUser failed: User cannot be null");
                throw new ArgumentNullException(nameof(user));
            }

            _userRepository.Add(user);
            _auditService.LogAction(user.UserId, "CreateUser", $"User created with ID {user.UserId}");
            _logger.LogInformation("User created with ID {UserId}", user.UserId);
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
                _auditService.LogAction(userId, "GetUserById", $"Retrieved user with ID {userId}");
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
            _auditService.LogAction(user.UserId, "UpdateUser", $"User updated with ID {user.UserId}");
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
            _auditService.LogAction(userId, "DeleteUser", $"User deleted with ID {userId}");
            _logger.LogInformation("User with ID {UserId} deleted", userId);
        }
    }
}