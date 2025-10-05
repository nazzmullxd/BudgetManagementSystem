using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;


namespace Database.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly BudgetManagementContext _context;

        public ExpenseCategoryRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(ExpenseCategory category)
        {
            _context.ExpenseCategories.Add(category);
            _context.SaveChanges();
        }

        public List<ExpenseCategory>? GetByUserId(string userId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .Where(ec => ec.UserId == userId)
                .ToList();
        }

        public ExpenseCategory? GetById(string categoryId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .FirstOrDefault(ec => ec.ExpenseCategoryId == categoryId);
        }

        public void Update(ExpenseCategory category)
        {
            _context.ExpenseCategories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(ExpenseCategory category)
        {
            _context.ExpenseCategories.Remove(category);
            _context.SaveChanges();
        }

        // Async methods
        public async Task AddAsync(ExpenseCategory category)
        {
            await _context.ExpenseCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ExpenseCategory>?> GetByUserIdAsync(string userId)
        {
            return await _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .Where(ec => ec.UserId == userId)
                .ToListAsync();
        }

        public async Task<ExpenseCategory?> GetByIdAsync(string categoryId)
        {
            return await _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .FirstOrDefaultAsync(ec => ec.ExpenseCategoryId == categoryId);
        }

        public async Task UpdateAsync(ExpenseCategory category)
        {
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ExpenseCategory category)
        {
            _context.ExpenseCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}