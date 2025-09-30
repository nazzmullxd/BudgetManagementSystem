using Database.Model;

namespace Business.Services
{
    public interface IReportService
    {
        (decimal totalExpenses, decimal totalIncome, decimal netAmount) GetFinancialSummary(string userId, DateTime startDate, DateTime endDate);
        Dictionary<string, decimal> GetExpensesByCategory(string userId, DateTime startDate, DateTime endDate);
        IEnumerable<TrackExpense> GetTopExpenses(string userId, int count, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<TrackIncome> GetIncomeHistory(string userId, DateTime startDate, DateTime endDate);
    }
}