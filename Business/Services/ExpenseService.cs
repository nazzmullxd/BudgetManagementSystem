using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ExpenseService : BaseService, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseCategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITransactionTagRepository _transactionTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IExpenseCategoryRepository categoryRepository,
            ICurrencyRepository currencyRepository,
            ITransactionTagRepository transactionTagRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<ExpenseService> logger)
            : base(userRepository, auditService)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _transactionTagRepository = transactionTagRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public void AddExpense(string userId, string itemName, decimal itemPrice, int quantity, string categoryId, string currencyId, DateTime transactionDate, List<string>? tagIds = null)
        {
            _logger.LogInformation("Adding expense for user {UserId}: {ItemName}", userId, itemName);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(itemName))
            {
                _logger.LogError("AddExpense failed: Item name is required for user {UserId}", userId);
                throw new ArgumentException("Item name is required.");
            }

            if (itemPrice <= 0)
            {
                _logger.LogError("AddExpense failed: Item price must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Item price must be greater than zero.");
            }

            if (quantity <= 0)
            {
                _logger.LogError("AddExpense failed: Quantity must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null || category.UserId != userId)
            {
                _logger.LogError("AddExpense failed: Invalid category ID {CategoryId} for user {UserId}", categoryId, userId);
                throw new ArgumentException("Invalid category ID.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("AddExpense failed: Invalid currency ID {CurrencyId} for user {UserId}", currencyId, userId);
                throw new ArgumentException("Invalid currency ID.");
            }

            if (tagIds != null && tagIds.Any())
            {
                var userTags = _tagRepository.GetByUserId(userId).Select(t => t.TagId).ToList();
                var invalidTags = tagIds.Where(tid => !userTags.Contains(tid)).ToList();
                if (invalidTags.Any())
                {
                    _logger.LogError("AddExpense failed: Invalid tag IDs {InvalidTags} for user {UserId}", string.Join(", ", invalidTags), userId);
                    throw new ArgumentException($"Invalid tag IDs: {string.Join(", ", invalidTags)}");
                }
            }

            var expense = new TrackExpense
            {
                UserId = userId,
                ItemName = itemName,
                ItemPrice = itemPrice,
                Quantity = quantity,
                ExpenseCategoryId = categoryId,
                CurrencyId = currencyId,
                TransactionDate = transactionDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _expenseRepository.Add(expense);

            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    var transactionTag = new TransactionTag
                    {
                        TrackExpenseId = expense.TrackExpenseId,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _transactionTagRepository.Add(transactionTag);
                }
            }

            _auditService.LogAction(userId, "AddExpense", $"Added expense: {itemName}, Total Cost: {expense.TotalCost}");
            _logger.LogInformation("Expense added for user {UserId}: {ItemName}", userId, itemName);
        }

        public void UpdateExpense(string expenseId, string itemName, decimal itemPrice, int quantity, string categoryId, string currencyId, DateTime transactionDate, List<string>? tagIds = null)
        {
            _logger.LogInformation("Updating expense {ExpenseId}", expenseId);

            var expense = _expenseRepository.GetByUserId(null).FirstOrDefault(e => e.TrackExpenseId == expenseId);
            if (expense == null)
            {
                _logger.LogError("UpdateExpense failed: Expense {ExpenseId} not found", expenseId);
                throw new ArgumentException("Expense not found.");
            }

            ValidateUser(expense.UserId);

            if (string.IsNullOrWhiteSpace(itemName))
            {
                _logger.LogError("UpdateExpense failed: Item name is required for expense {ExpenseId}", expenseId);
                throw new ArgumentException("Item name is required.");
            }

            if (itemPrice <= 0)
            {
                _logger.LogError("UpdateExpense failed: Item price must be greater than zero for expense {ExpenseId}", expenseId);
                throw new ArgumentException("Item price must be greater than zero.");
            }

            if (quantity <= 0)
            {
                _logger.LogError("UpdateExpense failed: Quantity must be greater than zero for expense {ExpenseId}", expenseId);
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null || category.UserId != expense.UserId)
            {
                _logger.LogError("UpdateExpense failed: Invalid category ID {CategoryId} for expense {ExpenseId}", categoryId, expenseId);
                throw new ArgumentException("Invalid category ID.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("UpdateExpense failed: Invalid currency ID {CurrencyId} for expense {ExpenseId}", currencyId, expenseId);
                throw new ArgumentException("Invalid currency ID.");
            }

            if (tagIds != null && tagIds.Any())
            {
                var userTags = _tagRepository.GetByUserId(expense.UserId).Select(t => t.TagId).ToList();
                var invalidTags = tagIds.Where(tid => !userTags.Contains(tid)).ToList();
                if (invalidTags.Any())
                {
                    _logger.LogError("UpdateExpense failed: Invalid tag IDs {InvalidTags} for expense {ExpenseId}", string.Join(", ", invalidTags), expenseId);
                    throw new ArgumentException($"Invalid tag IDs: {string.Join(", ", invalidTags)}");
                }
            }

            expense.ItemName = itemName;
            expense.ItemPrice = itemPrice;
            expense.Quantity = quantity;
            expense.ExpenseCategoryId = categoryId;
            expense.CurrencyId = currencyId;
            expense.TransactionDate = transactionDate;
            expense.UpdatedAt = DateTime.Now;

            _expenseRepository.Update(expense);

            var existingTags = _transactionTagRepository.GetByExpenseId(expenseId);
            foreach (var tag in existingTags)
            {
                _transactionTagRepository.Delete(tag.TransactionTagId);
            }

            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    var transactionTag = new TransactionTag
                    {
                        TrackExpenseId = expenseId,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _transactionTagRepository.Add(transactionTag);
                }
            }

            _auditService.LogAction(expense.UserId, "UpdateExpense", $"Updated expense: {itemName}, Total Cost: {expense.TotalCost}");
            _logger.LogInformation("Expense {ExpenseId} updated", expenseId);
        }

        public void DeleteExpense(string expenseId)
        {
            _logger.LogInformation("Deleting expense {ExpenseId}", expenseId);

            var expense = _expenseRepository.GetByUserId(null).FirstOrDefault(e => e.TrackExpenseId == expenseId);
            if (expense == null)
            {
                _logger.LogError("DeleteExpense failed: Expense {ExpenseId} not found", expenseId);
                throw new ArgumentException("Expense not found.");
            }

            ValidateUser(expense.UserId);

            var tags = _transactionTagRepository.GetByExpenseId(expenseId);
            foreach (var tag in tags)
            {
                _transactionTagRepository.Delete(tag.TransactionTagId);
            }

            _expenseRepository.Delete(expense);

            _auditService.LogAction(expense.UserId, "DeleteExpense", $"Deleted expense: {expense.ItemName}, Total Cost: {expense.TotalCost}");
            _logger.LogInformation("Expense {ExpenseId} deleted", expenseId);
        }

        public List<TrackExpense> GetExpensesByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Retrieving expenses for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            List<TrackExpense> expenses;
            if (startDate == null || endDate == null)
            {
                expenses = _expenseRepository.GetByUserId(userId);
            }
            else
            {
                expenses = _expenseRepository.GetByUserIdAndDateRange(userId, startDate.Value, endDate.Value);
            }

            _logger.LogInformation("Retrieved {Count} expenses for user {UserId}", expenses.Count, userId);
            return expenses;
        }
    }
}