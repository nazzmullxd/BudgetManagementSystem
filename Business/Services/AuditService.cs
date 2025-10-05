using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class AuditService : BaseService, IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(
            IAuditLogRepository auditLogRepository,
            IUserRepository userRepository,
            ILogger<AuditService> logger)
            : base(userRepository, null)
        {
            _auditLogRepository = auditLogRepository;
            _logger = logger;
        }

        public void LogAction(string userId, string action, string details)
        {
            _logger.LogInformation("Logging action for user {UserId}: {Action}", userId, action);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(action))
            {
                _logger.LogError("LogAction failed: Action is required for user {UserId}", userId);
                throw new ArgumentException("Action is required.");
            }

            if (string.IsNullOrWhiteSpace(details))
            {
                _logger.LogError("LogAction failed: Details are required for user {UserId}", userId);
                throw new ArgumentException("Details are required.");
            }

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityType = "User",
                EntityId = userId,
                Details = details,
                Timestamp = DateTime.Now
            };

            _auditLogRepository.Add(auditLog);
            _logger.LogInformation("Action logged for user {UserId}: {Action}", userId, action);
        }

        public List<AuditLog> GetAuditLogsByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Retrieving audit logs for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var auditLogs = _auditLogRepository.GetByUserId(userId);
            List<AuditLog> filteredLogs;
            
            if (auditLogs == null)
            {
                filteredLogs = new List<AuditLog>();
            }
            else if (startDate == null || endDate == null)
            {
                filteredLogs = auditLogs.ToList();
            }
            else
            {
                filteredLogs = auditLogs
                    .Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate)
                    .ToList();
            }

            _logger.LogInformation("Retrieved {Count} audit logs for user {UserId}", filteredLogs.Count, userId);
            return filteredLogs;
        }
    }
}
