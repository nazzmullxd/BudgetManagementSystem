using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public abstract class BaseService
    {
        protected readonly IUserRepository _userRepository;
        protected readonly IAuditService? _auditService;

        protected BaseService(IUserRepository userRepository, IAuditService? auditService = null)
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
            logger.LogInformation("User {UserId} performed action: {Action} - {Details}", userId, action, details);
            
            if (_auditService != null)
            {
                try
                {
                    _auditService.LogAction(userId, action, details);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to log audit action for user {UserId}", userId);
                }
            }
        }
    }
}