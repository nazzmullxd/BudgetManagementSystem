using Database.Model;

namespace Business.Services
{
    public interface ICategoryService
    {
        void CreateCategory(string userId, string categoryName, string? description = null);
        Task CreateCategoryAsync(string userId, string categoryName, string? description = null);
        void UpdateCategory(string categoryId, string categoryName, string? description = null);
        Task UpdateCategoryAsync(string categoryId, string categoryName, string? description = null);
        void DeleteCategory(string categoryId);
        Task DeleteCategoryAsync(string categoryId);
        IEnumerable<ExpenseCategory> GetCategoriesForUser(string userId);
        Task<IEnumerable<ExpenseCategory>> GetCategoriesForUserAsync(string userId);
        ExpenseCategory? GetCategoryById(string categoryId);
        Task<ExpenseCategory?> GetCategoryByIdAsync(string categoryId);
    }
}