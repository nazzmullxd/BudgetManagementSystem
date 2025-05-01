using Database.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class BudgetService : BaseService, IBudgetService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IBudgetGoalRepository _budgetGoalRepository;
        private readonly IExpenseCategoryRepository _categoryRepository;
        private readonly IBudgetAlertsRepository _budgetAlertsRepository;
        private readonly ILogger<BudgetService> _logger;

        public BudgetService(
            IExpenseRepository expenseRepository,
            IBudgetGoalRepository budgetGoalRepository,
            IExpenseCategoryRepository categoryRepository,
            IBudgetAlertsRepository budgetAlertsRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<BudgetService> logger)
            : base(userRepository, auditService)
        {
            _expenseRepository = expenseRepository;
            _budgetGoalRepository = budgetGoalRepository;
            _categoryRepository = categoryRepository;
            _budgetAlertsRepository = budgetAlertsRepository;
            _logger = logger;
        }

        public void SetBudgetGoal(string userId, string categoryId, decimal amount, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Setting budget goal for user {UserId}, category {CategoryId}", userId, categoryId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("SetBudgetGoal failed: Category ID is required for user {UserId}", userId);
                throw new ArgumentException("Category ID is required.");
            }

            if (amount <= 0)
            {
                _logger.LogError("SetBudgetGoal failed: Amount must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Amount must be greater than zero.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null || category.UserId != userId)
            {
                _logger.LogError("SetBudgetGoal failed: Category {CategoryId} not found or does not belong to user {UserId}", categoryId, userId);
                throw new ArgumentException("Category not found or does not belong to the user.");
            }

            var budgetGoal = new BudgetGoal
            {
                UserId = userId,
                ExpenseCategoryId = categoryId,
                Amount = amount,
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _budgetGoalRepository.Add(budgetGoal);
            LogAction(_logger, userId, "Budget Goal Set", $"Budget goal set for category {category.CategoryName}: {amount}");
        }

        public bool CheckBudgetExceeded(string userId, string categoryId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Checking if budget exceeded for user {UserId}, category {CategoryId}", userId, categoryId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("CheckBudgetExceeded failed: Category ID is required for user {UserId}", userId);
                throw new ArgumentException("Category ID is required.");
            }

            var budgetGoals = _budgetGoalRepository.GetByUserId(userId)
                .Where(bg => bg.ExpenseCategoryId == categoryId &&
                             bg.StartDate <= endDate &&
                             bg.EndDate >= startDate)
                .ToList();

            if (!budgetGoals.Any())
            {
                _logger.LogInformation("No budget goal found for user {UserId}, category {CategoryId}", userId, categoryId);
                return false;
            }

            var budgetGoal = budgetGoals.First();
            var totalExpenses = GetTotalExpensesInCategory(userId, categoryId, startDate, endDate);

            bool exceeded = totalExpenses > budgetGoal.Amount;
            _logger.LogInformation("Budget check for user {UserId}, category {CategoryId}: Exceeded = {Exceeded}, Total Expenses = {TotalExpenses}, Budget = {Budget}", userId, categoryId, exceeded, totalExpenses, budgetGoal.Amount);
            return exceeded;
        }

        public decimal GetTotalExpensesInCategory(string userId, string categoryId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Calculating total expenses for user {UserId}, category {CategoryId}", userId, categoryId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("GetTotalExpensesInCategory failed: Category ID is required for user {UserId}", userId);
                throw new ArgumentException("Category ID is required.");
            }

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate)
                .Where(e => e.ExpenseCategoryId == categoryId)
                .ToList();

            var total = expenses.Sum(e => e.TotalCost);
            _logger.LogInformation("Total expenses for user {UserId}, category {CategoryId}: {Total}", userId, categoryId, total);
            return total;
        }

        public void CheckAndCreateBudgetAlert(string userId, string categoryId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Checking and creating budget alert for user {UserId}, category {CategoryId}", userId, categoryId);

            if (CheckBudgetExceeded(userId, categoryId, startDate, endDate))
            {
                var category = _categoryRepository.GetById(categoryId);
                var totalExpenses = GetTotalExpensesInCategory(userId, categoryId, startDate, endDate);
                var budgetGoal = _budgetGoalRepository.GetByUserId(userId)
                    .First(bg => bg.ExpenseCategoryId == categoryId &&
                                 bg.StartDate <= endDate &&
                                 bg.EndDate >= startDate);

                var alert = new BudgetAlerts
                {
                    UserId = userId,
                    Message = $"Budget exceeded for category '{category.CategoryName}'. Budget: {budgetGoal.Amount}, Spent: {totalExpenses}",
                    AlertDate = DateTime.Now,
                    IsSent = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _budgetAlertsRepository.Add(alert);
                LogAction(_logger, userId, "Budget Alert Created", alert.Message);
            }
        }
    }
}