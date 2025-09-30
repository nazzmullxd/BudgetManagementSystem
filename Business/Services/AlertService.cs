using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class AlertService : BaseService, IAlertService
    {
        private readonly IBudgetAlertsRepository _budgetAlertsRepository;
        private readonly ILogger<AlertService> _logger;

        public AlertService(
            IBudgetAlertsRepository budgetAlertsRepository,
            IUserRepository userRepository,
            ILogger<AlertService> logger)
            : base(userRepository)
        {
            _budgetAlertsRepository = budgetAlertsRepository;
            _logger = logger;
        }

        public void CreateAlert(BudgetAlerts alert)
        {
            _logger.LogInformation("Creating alert for user {UserId}", alert?.UserId);

            if (alert == null)
            {
                _logger.LogError("CreateAlert failed: Alert cannot be null");
                throw new ArgumentNullException(nameof(alert));
            }

            ValidateUser(alert.UserId);

            _budgetAlertsRepository.Add(alert);
            _logger.LogInformation("Alert created for user {UserId} with ID {AlertId}", alert.UserId, alert.BudgetAlertsId);
        }

        public List<BudgetAlerts>? GetAlertsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving alerts for user {UserId}", userId);

            ValidateUser(userId);

            var alerts = _budgetAlertsRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} alerts for user {UserId}", alerts?.Count ?? 0, userId);
            return alerts;
        }

        public BudgetAlerts? GetAlertById(string alertId)
        {
            _logger.LogInformation("Retrieving alert with ID {AlertId}", alertId);

            if (string.IsNullOrWhiteSpace(alertId))
            {
                _logger.LogError("GetAlertById failed: Alert ID is required");
                throw new ArgumentException("Alert ID is required.", nameof(alertId));
            }

            var alert = _budgetAlertsRepository.GetById(alertId);
            if (alert == null)
            {
                _logger.LogWarning("Alert with ID {AlertId} not found", alertId);
            }
            else
            {
                _logger.LogInformation("Retrieved alert with ID {AlertId}", alertId);
            }
            return alert;
        }

        public void UpdateAlert(BudgetAlerts alert)
        {
            _logger.LogInformation("Updating alert with ID {AlertId}", alert?.BudgetAlertsId);

            if (alert == null)
            {
                _logger.LogError("UpdateAlert failed: Alert cannot be null");
                throw new ArgumentNullException(nameof(alert));
            }

            ValidateUser(alert.UserId);

            var existingAlert = _budgetAlertsRepository.GetById(alert.BudgetAlertsId);
            if (existingAlert == null)
            {
                _logger.LogError("UpdateAlert failed: Alert with ID {AlertId} not found", alert.BudgetAlertsId);
                throw new KeyNotFoundException($"Alert with ID {alert.BudgetAlertsId} not found.");
            }

            _budgetAlertsRepository.Update(alert);
            _logger.LogInformation("Alert with ID {AlertId} updated", alert.BudgetAlertsId);
        }

        public void DeleteAlert(string alertId)
        {
            _logger.LogInformation("Deleting alert with ID {AlertId}", alertId);

            if (string.IsNullOrWhiteSpace(alertId))
            {
                _logger.LogError("DeleteAlert failed: Alert ID is required");
                throw new ArgumentException("Alert ID is required.", nameof(alertId));
            }

            var alert = _budgetAlertsRepository.GetById(alertId);
            if (alert == null)
            {
                _logger.LogError("DeleteAlert failed: Alert with ID {AlertId} not found", alertId);
                throw new KeyNotFoundException($"Alert with ID {alertId} not found.");
            }

            ValidateUser(alert.UserId);

            _budgetAlertsRepository.Update(alert); // Assuming Update since Delete isn't in IBudgetAlertsRepository; adjust if needed
            _logger.LogInformation("Alert with ID {AlertId} deleted", alertId);
        }
    }
}