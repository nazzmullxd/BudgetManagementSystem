using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

namespace Business.Services
{
    public class IncomeService : BaseService, IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITransactionTagRepository _transactionTagRepository;
        private readonly ILogger<IncomeService> _logger;

        public IncomeService(
            IIncomeRepository incomeRepository,
            ICurrencyRepository currencyRepository,
            ITagRepository tagRepository,
            ITransactionTagRepository transactionTagRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<IncomeService> logger)
            : base(userRepository, auditService)
        {
            _incomeRepository = incomeRepository;
            _currencyRepository = currencyRepository;
            _tagRepository = tagRepository;
            _transactionTagRepository = transactionTagRepository;
            _logger = logger;
        }

        public void AddIncome(string userId, string source, decimal amount, string currencyId, DateTime incomeDate, List<string> tagIds = null)
        {
            _logger.LogInformation("Adding income for user {UserId}: {Source}", userId, source);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(source))
            {
                _logger.LogError("AddIncome failed: Source is required for user {UserId}", userId);
                throw new ArgumentException("Source is required.");
            }

            if (amount <= 0)
            {
                _logger.LogError("AddIncome failed: Amount must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("AddIncome failed: Currency ID is required for user {UserId}", userId);
                throw new ArgumentException("Currency ID is required.");
            }

            if (incomeDate > DateTime.Now)
            {
                _logger.LogError("AddIncome failed: Income date cannot be in the future for user {UserId}", userId);
                throw new ArgumentException("Income date cannot be in the future.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("AddIncome failed: Currency {CurrencyId} not found for user {UserId}", currencyId, userId);
                throw new ArgumentException("Currency not found.");
            }

            if (tagIds != null && tagIds.Any())
            {
                var tags = _tagRepository.GetByUserId(userId).Where(t => tagIds.Contains(t.TagId)).ToList();
                if (tags.Count != tagIds.Count)
                {
                    _logger.LogError("AddIncome failed: Invalid tags for user {UserId}", userId);
                    throw new ArgumentException("One or more tags are invalid or do not belong to the user.");
                }
            }

            var income = new TrackIncome
            {
                UserId = userId,
                Source = source,
                Amount = amount,
                CurrencyId = currencyId,
                IncomeDate = incomeDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _incomeRepository.Add(income);

            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    var transactionTag = new TransactionTag
                    {
                        TrackIncomeId = income.TrackIncomeId,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _transactionTagRepository.Add(transactionTag);
                }
            }

            LogAction(_logger, userId, "Income Added", $"Income {source} added with amount {amount}");
        }

        public void UpdateIncome(string incomeId, string source, decimal amount, string currencyId, DateTime incomeDate, List<string> tagIds = null)
        {
            _logger.LogInformation("Updating income {IncomeId}", incomeId);

            if (string.IsNullOrWhiteSpace(incomeId))
            {
                _logger.LogError("UpdateIncome failed: Income ID is required.");
                throw new ArgumentException("Income ID is required.");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                _logger.LogError("UpdateIncome failed: Source is required for income {IncomeId}", incomeId);
                throw new ArgumentException("Source is required.");
            }

            if (amount <= 0)
            {
                _logger.LogError("UpdateIncome failed: Amount must be greater than zero for income {IncomeId}", incomeId);
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("UpdateIncome failed: Currency ID is required for income {IncomeId}", incomeId);
                throw new ArgumentException("Currency ID is required.");
            }

            var income = _incomeRepository.GetByUserId(null)
                .FirstOrDefault(i => i.TrackIncomeId == incomeId);

            if (income == null)
            {
                _logger.LogError("UpdateIncome failed: Income {IncomeId} not found.", incomeId);
                throw new ArgumentException("Income not found.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("UpdateIncome failed: Currency {CurrencyId} not found for income {IncomeId}", currencyId, incomeId);
                throw new ArgumentException("Currency not found.");
            }

            if (tagIds != null && tagIds.Any())
            {
                var tags = _tagRepository.GetByUserId(income.UserId).Where(t => tagIds.Contains(t.TagId)).ToList();
                if (tags.Count != tagIds.Count)
                {
                    _logger.LogError("UpdateIncome failed: Invalid tags for income {IncomeId}", incomeId);
                    throw new ArgumentException("One or more tags are invalid or do not belong to the user.");
                }
            }

            income.Source = source;
            income.Amount = amount;
            income.CurrencyId = currencyId;
            income.IncomeDate = incomeDate;
            income.UpdatedAt = DateTime.Now;

            _incomeRepository.Update(income);

            var existingTags = _transactionTagRepository.GetByIncomeId(incomeId);
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
                        TrackIncomeId = income.TrackIncomeId,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _transactionTagRepository.Add(transactionTag);
                }
            }

            LogAction(_logger, income.UserId, "Income Updated", $"Income {incomeId} updated to {source}");
        }

        public void DeleteIncome(string incomeId)
        {
            _logger.LogInformation("Deleting income {IncomeId}", incomeId);

            if (string.IsNullOrWhiteSpace(incomeId))
            {
                _logger.LogError("DeleteIncome failed: Income ID is required.");
                throw new ArgumentException("Income ID is required.");
            }

            var income = _incomeRepository.GetByUserId(null)
                .FirstOrDefault(i => i.TrackIncomeId == incomeId);

            if (income == null)
            {
                _logger.LogError("DeleteIncome failed: Income {IncomeId} not found.", incomeId);
                throw new ArgumentException("Income not found.");
            }

            var transactionTags = _transactionTagRepository.GetByIncomeId(incomeId);
            foreach (var tag in transactionTags)
            {
                _transactionTagRepository.Delete(tag.TransactionTagId);
            }

            _incomeRepository.Delete(income);
            LogAction(_logger, income.UserId, "Income Deleted", $"Income {incomeId} deleted");
        }

        public List<TrackIncome> GetIncomesByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Retrieving incomes for user {UserId}", userId);

            ValidateUser(userId);
            ValidateDateRange(startDate, endDate);

            List<TrackIncome> incomes;
            if (startDate == null || endDate == null)
            {
                incomes = _incomeRepository.GetByUserId(userId);
            }
            else
            {
                incomes = _incomeRepository.GetByUserIdAndDateRange(userId, startDate.Value, endDate.Value);
            }

            _logger.LogInformation("Retrieved {Count} incomes for user {UserId}", incomes.Count, userId);
            return incomes;
        }
    }
}