using Database.Model;

namespace Business.Services
{
    public interface IExpenseCategoryService
    {
        void CreateCategory(ExpenseCategory category);
        List<ExpenseCategory>? GetCategoriesByUserId(string userId);
        ExpenseCategory? GetCategoryById(string categoryId);
        void UpdateCategory(ExpenseCategory category);
        void DeleteCategory(string categoryId);
    }
}