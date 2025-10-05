using Database.Model;

namespace Database.Repositories
{
    public interface IExpenseRepository
    {
        void Add(TrackExpense expense);
        Task AddAsync(TrackExpense expense);
        List<TrackExpense>? GetByUserId(string userId);
        Task<List<TrackExpense>?> GetByUserIdAsync(string userId);
        List<TrackExpense>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        Task<List<TrackExpense>?> GetByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        TrackExpense? GetById(string expenseId);
        Task<TrackExpense?> GetByIdAsync(string expenseId);
        void Update(TrackExpense expense);
        Task UpdateAsync(TrackExpense expense);
        void Delete(TrackExpense expense);
        Task DeleteAsync(TrackExpense expense);
    }
}