using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

namespace Business.Services
{
    public class DuesService : BaseService, IDuesService
    {
        private readonly IDuesManagementRepository _duesRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<DuesService> _logger;

        public DuesService(
            IDuesManagementRepository duesRepository,
            ICurrencyRepository currencyRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<DuesService> logger)
            : base(userRepository, auditService)
        {
            _duesRepository = duesRepository;
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public void CreateDue(string userId, string description, decimal amount, string currencyId, DateTime dueDate)
        {
            _logger.LogInformation("Creating due for user {UserId}: {Description}", userId, description);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("CreateDue failed: Description is required for user {UserId}", userId);
                throw new ArgumentException("Description is required.");
            }

            if (amount <= 0)
            {
                _logger.LogError("CreateDue failed: Amount must be greater than zero for user {UserId}", userId);
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (dueDate < DateTime.Now)
            {
                _logger.LogError("CreateDue failed: Due date cannot be in the past for user {UserId}", userId);
                throw new ArgumentException("Due date cannot be in the past.");
            }

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("CreateDue failed: Currency ID is required for user {UserId}", userId);
                throw new ArgumentException("Currency ID is required.");
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("CreateDue failed: Currency {CurrencyId} not found for user {UserId}", currencyId, userId);
                throw new ArgumentException("Currency not found.");
            }

            var due = new DuesManagement
            {
                UserId = userId,
                Description = description,
                Amount = amount,
                CurrencyId = currencyId,
                DueDate = dueDate,
                IsPaid = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _duesRepository.Add(due);
            LogAction(_logger, userId, "Due Created", $"Due {description} created with amount {amount}");
        }

        public void MarkDueAsPaid(string dueId)
        {
            _logger.LogInformation("Marking due {DueId} as paid", dueId);

            if (string.IsNullOrWhiteSpace(dueId))
            {
                _logger.LogError("MarkDueAsPaid failed: Due ID is required.");
                throw new ArgumentException("Due ID is required.");
            }

            var due = _duesRepository.GetByUserId(null)
                .FirstOrDefault(d => d.DuesManagementId == dueId);

            if (due == null)
            {
                _logger.LogError("MarkDueAsPaid failed: Due {DueId} not found.", dueId);
                throw new ArgumentException("Due not found.");
            }

            if (due.IsPaid)
            {
                _logger.LogError("MarkDueAsPaid failed: Due {DueId} is already marked as paid.", dueId);
                throw new InvalidOperationException("Due is already marked as paid.");
            }

            due.IsPaid = true;
            due.PaidAt = DateTime.Now;
            due.UpdatedAt = DateTime.Now;

            _duesRepository.Update(due);
            LogAction(_logger, due.UserId, "Due Marked As Paid", $"Due {dueId} marked as paid");
        }

        public List<DuesManagement> GetDuesByUserId(string userId, bool includePaid = false)
        {
            _logger.LogInformation("Retrieving dues for user {UserId}, IncludePaid: {IncludePaid}", userId, includePaid);

            ValidateUser(userId);

            var dues = _duesRepository.GetByUserId(userId);
            if (!includePaid)
            {
                dues = dues.Where(d => !d.IsPaid).ToList();
            }

            _logger.LogInformation("Retrieved {Count} dues for user {UserId}", dues.Count, userId);
            return dues;
        }
    }
}