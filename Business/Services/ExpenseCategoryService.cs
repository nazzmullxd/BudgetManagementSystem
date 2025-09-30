using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ExpenseCategoryService : BaseService, IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly ILogger<ExpenseCategoryService> _logger;

        public ExpenseCategoryService(
            IExpenseCategoryRepository expenseCategoryRepository,
            IUserRepository userRepository,
            ILogger<ExpenseCategoryService> logger)
            : base(userRepository)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _logger = logger;
        }

        public void CreateCategory(ExpenseCategory category)
        {
            _logger.LogInformation("Creating expense category for user {UserId}", category?.UserId);

            if (category == null)
            {
                _logger.LogError("CreateCategory failed: Category cannot be null");
                throw new ArgumentNullException(nameof(category));
            }

            ValidateUser(category.UserId);

            _expenseCategoryRepository.Add(category);
            _logger.LogInformation("Expense category created for user {UserId} with ID {CategoryId}", category.UserId, category.ExpenseCategoryId);
        }

        public List<ExpenseCategory>? GetCategoriesByUserId(string userId)
        {
            _logger.LogInformation("Retrieving expense categories for user {UserId}", userId);

            ValidateUser(userId);

            var categories = _expenseCategoryRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} expense categories for user {UserId}", categories?.Count ?? 0, userId);
            return categories;
        }

        public ExpenseCategory? GetCategoryById(string categoryId)
        {
            _logger.LogInformation("Retrieving expense category with ID {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("GetCategoryById failed: Category ID is required");
                throw new ArgumentException("Category ID is required.", nameof(categoryId));
            }

            var category = _expenseCategoryRepository.GetById(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Expense category with ID {CategoryId} not found", categoryId);
            }
            else
            {
                _logger.LogInformation("Retrieved expense category with ID {CategoryId}", categoryId);
            }
            return category;
        }

        public void UpdateCategory(ExpenseCategory category)
        {
            _logger.LogInformation("Updating expense category with ID {CategoryId}", category?.ExpenseCategoryId);

            if (category == null)
            {
                _logger.LogError("UpdateCategory failed: Category cannot be null");
                throw new ArgumentNullException(nameof(category));
            }

            ValidateUser(category.UserId);

            var existingCategory = _expenseCategoryRepository.GetById(category.ExpenseCategoryId);
            if (existingCategory == null)
            {
                _logger.LogError("UpdateCategory failed: Expense category with ID {CategoryId} not found", category.ExpenseCategoryId);
                throw new KeyNotFoundException($"Expense category with ID {category.ExpenseCategoryId} not found.");
            }

            _expenseCategoryRepository.Update(category);
            _logger.LogInformation("Expense category with ID {CategoryId} updated", category.ExpenseCategoryId);
        }

        public void DeleteCategory(string categoryId)
        {
            _logger.LogInformation("Deleting expense category with ID {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("DeleteCategory failed: Category ID is required");
                throw new ArgumentException("Category ID is required.", nameof(categoryId));
            }

            var category = _expenseCategoryRepository.GetById(categoryId);
            if (category == null)
            {
                _logger.LogError("DeleteCategory failed: Expense category with ID {CategoryId} not found", categoryId);
                throw new KeyNotFoundException($"Expense category with ID {categoryId} not found.");
            }

            ValidateUser(category.UserId);

            _expenseCategoryRepository.Delete(category);
            _logger.LogInformation("Expense category with ID {CategoryId} deleted", categoryId);
        }
    }
}