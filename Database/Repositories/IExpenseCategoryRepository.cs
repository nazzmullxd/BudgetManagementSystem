using Database.Model;

namespace Database.Repositories
{
    public interface IExpenseCategoryRepository
    {
        void Add(ExpenseCategory category);
        Task AddAsync(ExpenseCategory category);
        List<ExpenseCategory>? GetByUserId(string userId);
        Task<List<ExpenseCategory>?> GetByUserIdAsync(string userId);
        ExpenseCategory? GetById(string categoryId);
        Task<ExpenseCategory?> GetByIdAsync(string categoryId);
        void Update(ExpenseCategory category);
        Task UpdateAsync(ExpenseCategory category);
        void Delete(ExpenseCategory category);
        Task DeleteAsync(ExpenseCategory category);
    }
}