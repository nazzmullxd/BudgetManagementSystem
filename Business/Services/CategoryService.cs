using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IExpenseCategoryRepository _categoryRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IExpenseCategoryRepository categoryRepository,
            IExpenseRepository expenseRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<CategoryService> logger)
            : base(userRepository, auditService)
        {
            _categoryRepository = categoryRepository;
            _expenseRepository = expenseRepository;
            _logger = logger;
        }

        public void CreateCategory(string userId, string categoryName, string? description = null)
        {
            _logger.LogInformation("Creating category for user {UserId}: {CategoryName}", userId, categoryName);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogError("CreateCategory failed: Category name is required for user {UserId}", userId);
                throw new ArgumentException("Category name is required.");
            }

            var existingCategory = _categoryRepository.GetByUserId(userId)?
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (existingCategory != null)
            {
                _logger.LogError("CreateCategory failed: Category {CategoryName} already exists for user {UserId}", categoryName, userId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            var category = new ExpenseCategory
            {
                CategoryName = categoryName,
                CategoryDescription = description ?? string.Empty,
                UserId = userId
            };

            _categoryRepository.Add(category);
            LogAction(_logger, userId, "Category Created", $"Category {categoryName} created");
        }

        public void UpdateCategory(string categoryId, string categoryName, string? description = null)
        {
            _logger.LogInformation("Updating category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("UpdateCategory failed: Category ID is required.");
                throw new ArgumentException("Category ID is required.");
            }

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogError("UpdateCategory failed: Category name is required for category {CategoryId}", categoryId);
                throw new ArgumentException("Category name is required.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
            {
                _logger.LogError("UpdateCategory failed: Category {CategoryId} not found.", categoryId);
                throw new ArgumentException("Category not found.");
            }

            var existingCategory = _categoryRepository.GetByUserId(category.UserId)?
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase) && c.ExpenseCategoryId != categoryId);

            if (existingCategory != null)
            {
                _logger.LogError("UpdateCategory failed: Category {CategoryName} already exists for user {UserId}", categoryName, category.UserId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            category.CategoryName = categoryName;
            category.CategoryDescription = description ?? category.CategoryDescription;

            _categoryRepository.Update(category);
            LogAction(_logger, category.UserId, "Category Updated", $"Category {categoryId} updated to {categoryName}");
        }

        public void DeleteCategory(string categoryId)
        {
            _logger.LogInformation("Deleting category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("DeleteCategory failed: Category ID is required.");
                throw new ArgumentException("Category ID is required.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
            {
                _logger.LogError("DeleteCategory failed: Category {CategoryId} not found.", categoryId);
                throw new ArgumentException("Category not found.");
            }

            // Check if category is being used in any expenses
            var userExpenses = _expenseRepository.GetByUserId(category.UserId);
            var hasExpenses = userExpenses?.Any(e => e.ExpenseCategoryId == categoryId) ?? false;

            if (hasExpenses)
            {
                _logger.LogError("DeleteCategory failed: Category {CategoryId} is in use by expenses.", categoryId);
                throw new InvalidOperationException("Cannot delete category because it is associated with expenses.");
            }

            _categoryRepository.Delete(category);
            LogAction(_logger, category.UserId, "Category Deleted", $"Category {categoryId} deleted");
        }

        public IEnumerable<ExpenseCategory> GetCategoriesForUser(string userId)
        {
            _logger.LogInformation("Retrieving categories for user {UserId}", userId);

            ValidateUser(userId);

            var categories = _categoryRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} categories for user {UserId}", categories?.Count() ?? 0, userId);
            return categories ?? new List<ExpenseCategory>();
        }

        public ExpenseCategory? GetCategoryById(string categoryId)
        {
            _logger.LogInformation("Retrieving category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("Category ID is required.");
            }

            return _categoryRepository.GetById(categoryId);
        }

        // Async versions of the methods
        public async Task CreateCategoryAsync(string userId, string categoryName, string? description = null)
        {
            _logger.LogInformation("Creating category for user {UserId}: {CategoryName}", userId, categoryName);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogError("CreateCategoryAsync failed: Category name is required for user {UserId}", userId);
                throw new ArgumentException("Category name is required.");
            }

            var existingCategory = (await _categoryRepository.GetByUserIdAsync(userId))?
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (existingCategory != null)
            {
                _logger.LogError("CreateCategoryAsync failed: Category {CategoryName} already exists for user {UserId}", categoryName, userId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            var category = new ExpenseCategory
            {
                CategoryName = categoryName,
                CategoryDescription = description ?? string.Empty,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category);
            LogAction(_logger, userId, "Category Created", $"Category {categoryName} created");
        }

        public async Task UpdateCategoryAsync(string categoryId, string categoryName, string? description = null)
        {
            _logger.LogInformation("Updating category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("UpdateCategoryAsync failed: Category ID is required.");
                throw new ArgumentException("Category ID is required.");
            }

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogError("UpdateCategoryAsync failed: Category name is required for category {CategoryId}", categoryId);
                throw new ArgumentException("Category name is required.");
            }

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogError("UpdateCategoryAsync failed: Category {CategoryId} not found.", categoryId);
                throw new ArgumentException("Category not found.");
            }

            var existingCategory = (await _categoryRepository.GetByUserIdAsync(category.UserId))?
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase) && c.ExpenseCategoryId != categoryId);

            if (existingCategory != null)
            {
                _logger.LogError("UpdateCategoryAsync failed: Category {CategoryName} already exists for user {UserId}", categoryName, category.UserId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            category.CategoryName = categoryName;
            category.CategoryDescription = description ?? category.CategoryDescription;

            await _categoryRepository.UpdateAsync(category);
            LogAction(_logger, category.UserId, "Category Updated", $"Category {categoryId} updated to {categoryName}");
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            _logger.LogInformation("Deleting category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("DeleteCategoryAsync failed: Category ID is required.");
                throw new ArgumentException("Category ID is required.");
            }

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogError("DeleteCategoryAsync failed: Category {CategoryId} not found.", categoryId);
                throw new ArgumentException("Category not found.");
            }

            // Check if category is being used in any expenses
            var userExpenses = await _expenseRepository.GetByUserIdAsync(category.UserId);
            var hasExpenses = userExpenses?.Any(e => e.ExpenseCategoryId == categoryId) ?? false;

            if (hasExpenses)
            {
                _logger.LogError("DeleteCategoryAsync failed: Category {CategoryId} is in use by expenses.", categoryId);
                throw new InvalidOperationException("Cannot delete category because it is associated with expenses.");
            }

            await _categoryRepository.DeleteAsync(category);
            LogAction(_logger, category.UserId, "Category Deleted", $"Category {categoryId} deleted");
        }

        public async Task<IEnumerable<ExpenseCategory>> GetCategoriesForUserAsync(string userId)
        {
            _logger.LogInformation("Retrieving categories for user {UserId}", userId);

            ValidateUser(userId);

            var categories = await _categoryRepository.GetByUserIdAsync(userId);
            _logger.LogInformation("Retrieved {Count} categories for user {UserId}", categories?.Count() ?? 0, userId);
            return categories ?? new List<ExpenseCategory>();
        }

        public async Task<ExpenseCategory?> GetCategoryByIdAsync(string categoryId)
        {
            _logger.LogInformation("Retrieving category {CategoryId}", categoryId);

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("Category ID is required.");
            }

            return await _categoryRepository.GetByIdAsync(categoryId);
        }
    }
}