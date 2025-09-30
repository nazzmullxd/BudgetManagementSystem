using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class IncomeService : BaseService, IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<IncomeService> _logger;

        public IncomeService(
            IIncomeRepository incomeRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<IncomeService> logger)
            : base(userRepository)
        {
            _incomeRepository = incomeRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateIncome(TrackIncome income)
        {
            _logger.LogInformation("Creating income for user {UserId}", income?.UserId);

            if (income == null)
            {
                _logger.LogError("CreateIncome failed: Income cannot be null");
                throw new ArgumentNullException(nameof(income));
            }

            ValidateUser(income.UserId);

            _incomeRepository.Add(income);
            _auditService.LogAction(income.UserId, "CreateIncome", $"Income created with ID {income.IncomeId}");
            _logger.LogInformation("Income created for user {UserId} with ID {IncomeId}", income.UserId, income.IncomeId);
        }

        public List<TrackIncome>? GetIncomesByUserId(string userId)
        {
            _logger.LogInformation("Retrieving incomes for user {UserId}", userId);

            ValidateUser(userId);

            var incomes = _incomeRepository.GetByUserId(userId);
            _auditService.LogAction(userId, "GetIncomesByUserId", $"Retrieved {incomes?.Count ?? 0} incomes");
            _logger.LogInformation("Retrieved {Count} incomes for user {UserId}", incomes?.Count ?? 0, userId);
            return incomes;
        }

        public List<TrackIncome>? GetIncomesByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Retrieving incomes for user {UserId} between {StartDate} and {EndDate}", userId, startDate, endDate);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var incomes = _incomeRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            _auditService.LogAction(userId, "GetIncomesByUserIdAndDateRange", $"Retrieved {incomes?.Count ?? 0} incomes between {startDate} and {endDate}");
            _logger.LogInformation("Retrieved {Count} incomes for user {UserId} between {StartDate} and {EndDate}", incomes?.Count ?? 0, userId, startDate, endDate);
            return incomes;
        }

        public TrackIncome? GetIncomeById(string incomeId)
        {
            _logger.LogInformation("Retrieving income with ID {IncomeId}", incomeId);

            if (string.IsNullOrWhiteSpace(incomeId))
            {
                _logger.LogError("GetIncomeById failed: Income ID is required");
                throw new ArgumentException("Income ID is required.", nameof(incomeId));
            }

            var income = _incomeRepository.GetByUserId(null)?.FirstOrDefault(i => i.IncomeId == incomeId);
            if (income == null)
            {
                _logger.LogWarning("Income with ID {IncomeId} not found", incomeId);
            }
            else
            {
                _logger.LogInformation("Retrieved income with ID {IncomeId}", incomeId);
                _auditService.LogAction(income.UserId, "GetIncomeById", $"Retrieved income with ID {incomeId}");
            }
            return income;
        }

        public void UpdateIncome(TrackIncome income)
        {
            _logger.LogInformation("Updating income with ID {IncomeId}", income?.IncomeId);

            if (income == null)
            {
                _logger.LogError("UpdateIncome failed: Income cannot be null");
                throw new ArgumentNullException(nameof(income));
            }

            ValidateUser(income.UserId);

            var existingIncome = GetIncomeById(income.IncomeId);
            if (existingIncome == null)
            {
                _logger.LogError("UpdateIncome failed: Income with ID {IncomeId} not found for user {UserId}", income.IncomeId, income.UserId);
                throw new KeyNotFoundException($"Income with ID {income.IncomeId} not found.");
            }

            _incomeRepository.Update(income);
            _auditService.LogAction(income.UserId, "UpdateIncome", $"Income updated with ID {income.IncomeId}");
            _logger.LogInformation("Income with ID {IncomeId} updated", income.IncomeId);
        }

        public void DeleteIncome(string incomeId)
        {
            _logger.LogInformation("Deleting income with ID {IncomeId}", incomeId);

            if (string.IsNullOrWhiteSpace(incomeId))
            {
                _logger.LogError("DeleteIncome failed: Income ID is required");
                throw new ArgumentException("Income ID is required.", nameof(incomeId));
            }

            var income = GetIncomeById(incomeId);
            if (income == null)
            {
                _logger.LogError("DeleteIncome failed: Income with ID {IncomeId} not found", incomeId);
                throw new KeyNotFoundException($"Income with ID {incomeId} not found.");
            }

            ValidateUser(income.UserId);

            _incomeRepository.Delete(income);
            _auditService.LogAction(income.UserId, "DeleteIncome", $"Income deleted with ID {incomeId}");
            _logger.LogInformation("Income with ID {IncomeId} deleted", incomeId);
        }
    }
}