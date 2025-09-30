using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IExpenseCategoryRepository _categoryRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IExpenseRepository expenseRepository,
            IIncomeRepository incomeRepository,
            IExpenseCategoryRepository categoryRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<ReportService> logger)
            : base(userRepository, auditService)
        {
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public (decimal totalExpenses, decimal totalIncome, decimal netAmount) GetFinancialSummary(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Generating financial summary for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            var incomes = _incomeRepository.GetByUserIdAndDateRange(userId, startDate, endDate);

            var totalExpenses = expenses?.Sum(e => e.TotalCost) ?? 0;
            var totalIncome = incomes?.Sum(i => i.IncomeAmount) ?? 0;
            var netAmount = totalIncome - totalExpenses;

            _logger.LogInformation("Financial summary for user {UserId}: Total Expenses = {TotalExpenses}, Total Income = {TotalIncome}, Net Amount = {NetAmount}", userId, totalExpenses, totalIncome, netAmount);
            LogAction(_logger, userId, "Financial Summary Generated", $"Total Expenses: {totalExpenses}, Total Income: {totalIncome}, Net Amount: {netAmount}");
            return (totalExpenses, totalIncome, netAmount);
        }

        public IEnumerable<TrackExpense> GetTopExpenses(string userId, int count, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Retrieving top {Count} expenses for user {UserId}", count, userId);

            ValidateUser(userId);

            var expenses = startDate.HasValue && endDate.HasValue
                ? _expenseRepository.GetByUserIdAndDateRange(userId, startDate.Value, endDate.Value)
                : _expenseRepository.GetByUserId(userId);

            if (expenses == null) return new List<TrackExpense>();

            var topExpenses = expenses
                .OrderByDescending(e => e.TotalCost)
                .Take(count)
                .ToList();

            _logger.LogInformation("Retrieved {Count} top expenses for user {UserId}", topExpenses.Count, userId);
            LogAction(_logger, userId, "Top Expenses Retrieved", $"Retrieved top {count} expenses");
            return topExpenses;
        }

        public IEnumerable<TrackIncome> GetIncomeHistory(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Retrieving income history for user {UserId} from {StartDate} to {EndDate}", userId, startDate, endDate);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var incomes = _incomeRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            
            if (incomes == null) return new List<TrackIncome>();

            var sortedIncomes = incomes
                .OrderByDescending(i => i.IncomeDate)
                .ToList();

            _logger.LogInformation("Retrieved {Count} income records for user {UserId}", sortedIncomes.Count, userId);
            LogAction(_logger, userId, "Income History Retrieved", $"Retrieved {sortedIncomes.Count} income records");
            return sortedIncomes;
        }

        public Dictionary<string, decimal> GetExpensesByCategory(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Generating expenses by category report for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            var categories = _categoryRepository.GetByUserId(userId);

            var expensesByCategory = new Dictionary<string, decimal>();

            if (categories != null && expenses != null)
            {
                foreach (var category in categories)
                {
                    var categoryExpenses = expenses
                        .Where(e => e.ExpenseCategoryId == category.ExpenseCategoryId)
                        .Sum(e => e.TotalCost);

                    expensesByCategory[category.CategoryName] = categoryExpenses;
                }
            }

            _logger.LogInformation("Expenses by category report generated for user {UserId}", userId);
            LogAction(_logger, userId, "Expenses By Category Report Generated", "Report generated");
            return expensesByCategory;
        }
    }
}