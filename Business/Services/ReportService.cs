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

        public (decimal TotalExpenses, decimal TotalIncome) GetFinancialSummary(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Generating financial summary for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            var incomes = _incomeRepository.GetByUserIdAndDateRange(userId, startDate, endDate);

            var totalExpenses = expenses.Sum(e => e.TotalCost);
            var totalIncome = incomes.Sum(i => i.Amount);

            _logger.LogInformation("Financial summary for user {UserId}: Total Expenses = {TotalExpenses}, Total Income = {TotalIncome}", userId, totalExpenses, totalIncome);
            LogAction(_logger, userId, "Financial Summary Generated", $"Total Expenses: {totalExpenses}, Total Income: {totalIncome}");
            return (totalExpenses, totalIncome);
        }

        public Dictionary<string, decimal> GetExpensesByCategory(string userId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Generating expenses by category report for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            var expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate, endDate);
            var categories = _categoryRepository.GetByUserId(userId);

            var expensesByCategory = new Dictionary<string, decimal>();

            foreach (var category in categories)
            {
                var categoryExpenses = expenses
                    .Where(e => e.ExpenseCategoryId == category.ExpenseCategoryId)
                    .Sum(e => e.TotalCost);

                expensesByCategory[category.CategoryName] = categoryExpenses;
            }

            _logger.LogInformation("Expenses by category report generated for user {UserId}", userId);
            LogAction(_logger, userId, "Expenses By Category Report Generated", "Report generated");
            return expensesByCategory;
        }
    }
}