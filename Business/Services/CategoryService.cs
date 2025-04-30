using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

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

        public void CreateCategory(string userId, string categoryName, string description = null)
        {
            _logger.LogInformation("Creating category for user {UserId}: {CategoryName}", userId, categoryName);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogError("CreateCategory failed: Category name is required for user {UserId}", userId);
                throw new ArgumentException("Category name is required.");
            }

            var existingCategory = _categoryRepository.GetByUserId(userId)
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (existingCategory != null)
            {
                _logger.LogError("CreateCategory failed: Category {CategoryName} already exists for user {UserId}", categoryName, userId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            var category = new ExpenseCategory
            {
                UserId = userId,
                CategoryName = categoryName,
                CategoryDescription = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _categoryRepository.Add(category);
            LogAction(_logger, userId, "Category Created", $"Category {categoryName} created");
        }

        public void UpdateCategory(string categoryId, string categoryName, string description = null)
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

            var existingCategory = _categoryRepository.GetByUserId(category.UserId)
                .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase) && c.ExpenseCategoryId != categoryId);

            if (existingCategory != null)
            {
                _logger.LogError("UpdateCategory failed: Category {CategoryName} already exists for user {UserId}", categoryName, category.UserId);
                throw new ArgumentException("A category with this name already exists for the user.");
            }

            category.CategoryName = categoryName;
            category.CategoryDescription = description;
            category.UpdatedAt = DateTime.Now;

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

            var expenses = _expenseRepository.GetByUserId(category.UserId)
                .Any(e => e.ExpenseCategoryId == categoryId);

            if (expenses)
            {
                _logger.LogError("DeleteCategory failed: Category {CategoryId} is in use by expenses.", categoryId);
                throw new InvalidOperationException("Cannot delete category because it is associated with expenses.");
            }

            _categoryRepository.Delete(category);
            LogAction(_logger, category.UserId, "Category Deleted", $"Category {categoryId} deleted");
        }

        public List<ExpenseCategory> GetCategoriesByUserId(string userId)
        {
            _logger.LogInformation("Retrieving categories for user {UserId}", userId);

            ValidateUser(userId);

            var categories = _categoryRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} categories for user {UserId}", categories.Count, userId);
            return categories;
        }
    }
}