using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ExpenseService : BaseService, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<ExpenseService> logger)
            : base(userRepository)
        {
            _expenseRepository = expenseRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateExpense(TrackExpense expense)
        {
            _logger.LogInformation("Creating expense for user {UserId}", expense?.UserId);

            if (expense == null)
            {
                _logger.LogError("CreateExpense failed: Expense cannot be null");
                throw new ArgumentNullException(nameof(expense));
            }

            ValidateUser(expense.UserId);

            _expenseRepository.Add(expense);
            _auditService.LogAction(expense.UserId, "CreateExpense", $"Expense created with ID {expense.TrackExpenseId}");
            _logger.LogInformation("Expense created for user {UserId} with ID {TrackExpenseId}", expense.UserId, expense.TrackExpenseId);
        }

        public List<TrackExpense>? GetExpensesByUserId(string userId)
        {
            _logger.LogInformation("Retrieving expenses for user {UserId}", userId);

            ValidateUser(userId);

            var expenses = _expenseRepository.GetByUserId(userId);
            _auditService.LogAction(userId, "GetExpensesByUserId", $"Retrieved {expenses?.Count ?? 0} expenses");
            _logger.LogInformation("Retrieved {Count} expenses for user {UserId}", expenses?.Count ?? 0, userId);
            return expenses;
        }

        public List<TrackExpense>? GetExpensesByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Retrieving expenses for user {UserId} between {StartDate} and {EndDate}", userId, startDate, endDate);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            _auditService.LogAction(userId, "GetExpensesByUserIdAndDateRange", $"Retrieved {expenses?.Count ?? 0} expenses between {startDate} and {endDate}");
            _logger.LogInformation("Retrieved {Count} expenses for user {UserId} between {StartDate} and {EndDate}", expenses?.Count ?? 0, userId, startDate, endDate);
            return expenses;
        }

        public TrackExpense? GetExpenseById(string expenseId)
        {
            _logger.LogInformation("Retrieving expense with ID {TrackExpenseId}", expenseId);

            if (string.IsNullOrWhiteSpace(expenseId))
            {
                _logger.LogError("GetExpenseById failed: Expense ID is required");
                throw new ArgumentException("Expense ID is required.", nameof(expenseId));
            }

            // Note: IExpenseRepository does not have GetById; this will need to be added
            var expense = _expenseRepository.GetByUserId(null)?.FirstOrDefault(e => e.TrackExpenseId == expenseId);
            if (expense == null)
            {
                _logger.LogWarning("Expense with ID {TrackExpenseId} not found", expenseId);
            }
            else
            {
                _logger.LogInformation("Retrieved expense with ID {TrackExpenseId}", expenseId);
                _auditService.LogAction(expense.UserId, "GetExpenseById", $"Retrieved expense with ID {expenseId}");
            }
            return expense;
        }

        public void UpdateExpense(TrackExpense expense)
        {
            _logger.LogInformation("Updating expense with ID {TrackExpenseId}", expense?.TrackExpenseId);

            if (expense == null)
            {
                _logger.LogError("UpdateExpense failed: Expense cannot be null");
                throw new ArgumentNullException(nameof(expense));
            }

            ValidateUser(expense.UserId);

            var existingExpense = GetExpenseById(expense.TrackExpenseId);
            if (existingExpense == null)
            {
                _logger.LogError("UpdateExpense failed: Expense with ID {TrackExpenseId} not found for user {UserId}", expense.TrackExpenseId, expense.UserId);
                throw new KeyNotFoundException($"Expense with ID {expense.TrackExpenseId} not found.");
            }

            _expenseRepository.Update(expense);
            _auditService.LogAction(expense.UserId, "UpdateExpense", $"Expense updated with ID {expense.TrackExpenseId}");
            _logger.LogInformation("Expense with ID {TrackExpenseId} updated", expense.TrackExpenseId);
        }

        public void DeleteExpense(string expenseId)
        {
            _logger.LogInformation("Deleting expense with ID {TrackExpenseId}", expenseId);

            if (string.IsNullOrWhiteSpace(expenseId))
            {
                _logger.LogError("DeleteExpense failed: Expense ID is required");
                throw new ArgumentException("Expense ID is required.", nameof(expenseId));
            }

            var expense = GetExpenseById(expenseId);
            if (expense == null)
            {
                _logger.LogError("DeleteExpense failed: Expense with ID {TrackExpenseId} not found", expenseId);
                throw new KeyNotFoundException($"Expense with ID {expenseId} not found.");
            }

            ValidateUser(expense.UserId);

            _expenseRepository.Delete(expense);
            _auditService.LogAction(expense.UserId, "DeleteExpense", $"Expense deleted with ID {expenseId}");
            _logger.LogInformation("Expense with ID {TrackExpenseId} deleted", expenseId);
        }
    }
}