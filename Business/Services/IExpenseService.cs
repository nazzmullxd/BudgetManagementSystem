using Database.Model;

namespace Business.Services
{
    public interface IExpenseService
    {
        void CreateExpense(TrackExpense expense);
        Task CreateExpenseAsync(TrackExpense expense);
        List<TrackExpense>? GetExpensesByUserId(string userId);
        Task<List<TrackExpense>?> GetExpensesByUserIdAsync(string userId);
        List<TrackExpense>? GetExpensesByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        Task<List<TrackExpense>?> GetExpensesByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        TrackExpense? GetExpenseById(string expenseId);
        Task<TrackExpense?> GetExpenseByIdAsync(string expenseId);
        void UpdateExpense(TrackExpense expense);
        Task UpdateExpenseAsync(TrackExpense expense);
        void DeleteExpense(string expenseId);
        Task DeleteExpenseAsync(string expenseId);
    }
}