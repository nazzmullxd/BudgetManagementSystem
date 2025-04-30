using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

namespace Business.Services
{
    public class RecurringTransactionService : BaseService, IRecurringTransactionService
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<RecurringTransactionService> _logger;

        public RecurringTransactionService(
            IRecurringTransactionRepository recurringTransactionRepository,
            IExpenseService expenseService,
            IExpenseCategoryRepository categoryRepository,
            ICurrencyRepository currencyRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<RecurringTransactionService> logger)
            : base(userRepository, auditService)
        {
            _recurringTransactionRepository = recurringTransactionRepository;
            _expenseService = expenseService;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public void CreateRecurringTransaction(string userId, string description, decimal amount, string categoryId, string currencyId, DateTime startDate, DateTime? endDate, string frequency)
        {
            _logger.LogInformation("Creating recurring transaction for user {UserId}: {Description}", userId, description);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("CreateRecurringTransaction failed: Description is required for user {UserId}", userId);
                throw new ArgumentException("Description is required.");
            }

            if (amount <= 0)
            {
                _logger.LogError("CreateRecurringTransaction failed: Amount must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                _logger.LogError("CreateRecurringTransaction failed: Category ID is required for user {UserId}", userId);
                throw new ArgumentException("Category ID is required.");
            }

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("CreateRecurringTransaction failed: Currency ID is required for user {UserId}", userId);
                throw new ArgumentException("Currency ID is required.");
            }

            var category = _categoryRepository.GetById(categoryId);
            if (category == null || category.UserId != userId)
            {
                _logger.LogError("CreateRecurringTransaction failed: Category {CategoryId} not found or does not belong to user {UserId}", categoryId, userId);
                throw new ArgumentException("Category not found or does not belong to the user.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("CreateRecurringTransaction failed: Currency {CurrencyId} not found for user {UserId}", currencyId, userId);
                throw new ArgumentException("Currency not found.");
            }

            var validFrequencies = new[] { "Daily", "Weekly", "Monthly" };
            if (!validFrequencies.Contains(frequency))
            {
                _logger.LogError("CreateRecurringTransaction failed: Invalid frequency for user {UserId}. Must be 'Daily', 'Weekly', or 'Monthly'.", userId);
                throw new ArgumentException("Invalid frequency. Must be 'Daily', 'Weekly', or 'Monthly'.");
            }

            if (endDate.HasValue && endDate < startDate)
            {
                _logger.LogError("CreateRecurringTransaction failed: End date cannot be earlier than start date for user {UserId}", userId);
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            var recurringTransaction = new RecurringTransaction
            {
                UserId = userId,
                Description = description,
                Amount = amount,
                ExpenseCategoryId = categoryId,
                CurrencyId = currencyId,
                StartDate = startDate,
                EndDate = endDate,
                Frequency = frequency,
                LastProcessedDate = null,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _recurringTransactionRepository.Add(recurringTransaction);
            LogAction(_logger, userId, "Recurring Transaction Created", $"Recurring Transaction {description} created");
        }

        public void ProcessRecurringTransactions(string userId, DateTime currentDate)
        {
            _logger.LogInformation("Processing recurring transactions for user {UserId}", userId);

            ValidateUser(userId);

            var recurringTransactions = _recurringTransactionRepository.GetByUserId(userId);

            foreach (var transaction in recurringTransactions)
            {
                if (currentDate < transaction.StartDate || (transaction.EndDate.HasValue && currentDate > transaction.EndDate))
                {
                    continue;
                }

                DateTime nextProcessDate = transaction.LastProcessedDate?.Date ?? transaction.StartDate.Date;
                while (nextProcessDate <= currentDate)
                {
                    if (transaction.Frequency == "Daily")
                    {
                        nextProcessDate = nextProcessDate.AddDays(1);
                    }
                    else if (transaction.Frequency == "Weekly")
                    {
                        nextProcessDate = nextProcessDate.AddDays(7);
                    }
                    else if (transaction.Frequency == "Monthly")
                    {
                        nextProcessDate = nextProcessDate.AddMonths(1);
                    }

                    if (nextProcessDate > currentDate)
                    {
                        break;
                    }

                    _expenseService.AddExpense(
                        userId: transaction.UserId,
                        itemName: transaction.Description,
                        itemPrice: transaction.Amount,
                        quantity: 1,
                        categoryId: transaction.ExpenseCategoryId,
                        currencyId: transaction.CurrencyId,
                        transactionDate: nextProcessDate);

                    transaction.LastProcessedDate = nextProcessDate;
                    transaction.UpdatedAt = DateTime.Now;
                    _recurringTransactionRepository.Update(transaction);
                }
            }

            LogAction(_logger, userId, "Recurring Transactions Processed", $"Processed recurring transactions up to {currentDate}");
        }

        public List<RecurringTransaction> GetRecurringTransactionsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving recurring transactions for user {UserId}", userId);

            ValidateUser(userId);

            var transactions = _recurringTransactionRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} recurring transactions for user {UserId}", transactions.Count, userId);
            return transactions;
        }
    }
}