using Database.Model;

namespace Business.Services
{
    public interface ICategoryService
    {
        void CreateCategory(string userId, string categoryName, string description = "");
        void UpdateCategory(string categoryId, string categoryName, string description = "");
        void DeleteCategory(string categoryId);
        IEnumerable<ExpenseCategory> GetCategoriesForUser(string userId);
        ExpenseCategory? GetCategoryById(string categoryId);
    }
}