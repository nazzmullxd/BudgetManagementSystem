using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<UserService> logger)
            : base(userRepository, auditService)
        {
            _userRepo = userRepository;
            _logger = logger;
        }

        public void RegisterUser(string name, string email, string password)
        {
            _logger.LogInformation("Registering user with email: {Email}", email);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogError("RegisterUser failed: Name is required");
                throw new ArgumentException("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("RegisterUser failed: Email is required");
                throw new ArgumentException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("RegisterUser failed: Password is required");
                throw new ArgumentException("Password is required.");
            }

            var existingUser = _userRepo.GetByEmail(email);
            if (existingUser != null)
            {
                _logger.LogError("RegisterUser failed: Email {Email} is already in use", email);
                throw new ArgumentException("Email is already in use.");
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true
            };

            _userRepo.Add(user);
            _auditService.LogAction(user.UserId, "RegisterUser", $"User registered: {email}");
            _logger.LogInformation("User registered successfully: {Email}", email);
        }

        public User GetUserById(string userId)
        {
            _logger.LogInformation("Retrieving user by ID: {UserId}", userId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("GetUserById failed: User ID is required");
                throw new ArgumentException("User ID is required.");
            }

            var user = _userRepo.GetById(userId);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("GetUserById failed: User {UserId} not found or inactive", userId);
                throw new ArgumentException("User not found or inactive.");
            }

            _logger.LogInformation("User retrieved: {UserId}", userId);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            _logger.LogInformation("Retrieving user by email: {Email}", email);

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("GetUserByEmail failed: Email is required");
                throw new ArgumentException("Email is required.");
            }

            var user = _userRepo.GetByEmail(email);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("GetUserByEmail failed: User with email {Email} not found or inactive", email);
                throw new ArgumentException("User not found or inactive.");
            }

            _logger.LogInformation("User retrieved: {Email}", email);
            return user;
        }

        public List<User> GetAllUsers()
        {
            _logger.LogInformation("Retrieving all active users");

            var users = _userRepo.GetAll();
            var activeUsers = users.FindAll(u => u.IsActive);

            _logger.LogInformation("Retrieved {Count} active users", activeUsers.Count);
            return activeUsers;
        }

        public void UpdateUser(string userId, string name, string email, string password)
        {
            _logger.LogInformation("Updating user: {UserId}", userId);

            var user = _userRepo.GetById(userId);
            if (user == null || !user.IsActive)
            {
                _logger.LogError("UpdateUser failed: User {UserId} not found or inactive", userId);
                throw new ArgumentException("User not found or inactive.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogError("UpdateUser failed: Name is required for user {UserId}", userId);
                throw new ArgumentException("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("UpdateUser failed: Email is required for user {UserId}", userId);
                throw new ArgumentException("Email is required.");
            }

            var existingUser = _userRepo.GetByEmail(email);
            if (existingUser != null && existingUser.UserId != userId)
            {
                _logger.LogError("UpdateUser failed: Email {Email} is already in use by another user", email);
                throw new ArgumentException("Email is already in use by another user.");
            }

            user.Name = name;
            user.Email = email;
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = HashPassword(password);
            }
            user.UpdatedAt = DateTime.Now;

            _userRepo.Update(user);
            _auditService.LogAction(userId, "UpdateUser", $"User updated: {email}");
            _logger.LogInformation("User {UserId} updated successfully", userId);
        }

        public void DeactivateUser(string userId)
        {
            _logger.LogInformation("Deactivating user: {UserId}", userId);

            var user = _userRepo.GetById(userId);
            if (user == null || !user.IsActive)
            {
                _logger.LogError("DeactivateUser failed: User {UserId} not found or already inactive", userId);
                throw new ArgumentException("User not found or already inactive.");
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;

            _userRepo.Update(user);
            _auditService.LogAction(userId, "DeactivateUser", $"User deactivated: {user.Email}");
            _logger.LogInformation("User {UserId} deactivated successfully", userId);
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
