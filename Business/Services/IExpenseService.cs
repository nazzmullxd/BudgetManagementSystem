using Database.Model;

namespace Business.Services
{
    public interface IExpenseService
    {
        void CreateExpense(TrackExpense expense);
        List<TrackExpense>? GetExpensesByUserId(string userId);
        List<TrackExpense>? GetExpensesByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        TrackExpense? GetExpenseById(string expenseId);
        void UpdateExpense(TrackExpense expense);
        void DeleteExpense(string expenseId);
    }
}