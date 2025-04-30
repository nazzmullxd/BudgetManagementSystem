using BudgetManagementSystem.Repositories;
using Business.Interfeces;
using Microsoft.Extensions.Logging;
using System;

namespace Business.Services
{
    public abstract class BaseService
    {
        protected readonly IAuditService _auditService;
        protected readonly IUserRepository _userRepository;

        protected BaseService(IUserRepository userRepository, IAuditService auditService)
        {
            _userRepository = userRepository;
            _auditService = auditService;
        }

        protected void ValidateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID is required.");
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
        }

        protected void ValidateDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }
        }

        protected void LogAction(ILogger logger, string userId, string action, string details)
        {
            logger.LogInformation("{Action} for user {UserId}: {Details}", action, userId, details);
            _auditService.LogAction(userId, action, details);
        }
    }
}