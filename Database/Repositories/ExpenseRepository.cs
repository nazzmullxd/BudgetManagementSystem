using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;


namespace Database.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly BudgetManagementContext _context;

        public ExpenseRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(TrackExpense expense)
        {
            _context.TrackExpenses.Add(expense);
            _context.SaveChanges();
        }

        public List<TrackExpense>? GetByUserId(string userId)
        {
            return _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId)
                .ToList();
        }

        public List<TrackExpense>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
        {
            return _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId && e.TransactionDate >= startDate && e.TransactionDate <= endDate)
                .ToList();
        }

        public TrackExpense? GetById(string expenseId)
        {
            return _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefault(e => e.TrackExpenseId == expenseId);
        }

        public void Update(TrackExpense expense)
        {
            _context.TrackExpenses.Update(expense);
            _context.SaveChanges();
        }

        public void Delete(TrackExpense expense)
        {
            _context.TrackExpenses.Remove(expense);
            _context.SaveChanges();
        }

        // Async methods
        public async Task AddAsync(TrackExpense expense)
        {
            await _context.TrackExpenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TrackExpense>?> GetByUserIdAsync(string userId)
        {
            return await _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<TrackExpense>?> GetByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId && e.TransactionDate >= startDate && e.TransactionDate <= endDate)
                .ToListAsync();
        }

        public async Task<TrackExpense?> GetByIdAsync(string expenseId)
        {
            return await _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(e => e.TrackExpenseId == expenseId);
        }

        public async Task UpdateAsync(TrackExpense expense)
        {
            _context.TrackExpenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TrackExpense expense)
        {
            _context.TrackExpenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}