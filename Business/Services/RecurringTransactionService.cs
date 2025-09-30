using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RecurringTransactionService : BaseService, IRecurringTransactionService
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<RecurringTransactionService> _logger;

        public RecurringTransactionService(
            IRecurringTransactionRepository recurringTransactionRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<RecurringTransactionService> logger)
            : base(userRepository)
        {
            _recurringTransactionRepository = recurringTransactionRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateRecurringTransaction(RecurringTransaction transaction)
        {
            _logger.LogInformation("Creating recurring transaction for user {UserId}", transaction?.UserId);

            if (transaction == null)
            {
                _logger.LogError("CreateRecurringTransaction failed: Transaction cannot be null");
                throw new ArgumentNullException(nameof(transaction));
            }

            ValidateUser(transaction.UserId);

            _recurringTransactionRepository.Add(transaction);
            _auditService.LogAction(transaction.UserId, "CreateRecurringTransaction", $"Recurring transaction created with ID {transaction.TransactionId}");
            _logger.LogInformation("Recurring transaction created for user {UserId} with ID {TransactionId}", transaction.UserId, transaction.TransactionId);
        }

        public List<RecurringTransaction>? GetRecurringTransactionsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving recurring transactions for user {UserId}", userId);

            ValidateUser(userId);

            var transactions = _recurringTransactionRepository.GetByUserId(userId);
            _auditService.LogAction(userId, "GetRecurringTransactionsByUserId", $"Retrieved {transactions?.Count ?? 0} recurring transactions");
            _logger.LogInformation("Retrieved {Count} recurring transactions for user {UserId}", transactions?.Count ?? 0, userId);
            return transactions;
        }

        public RecurringTransaction? GetRecurringTransactionById(string transactionId)
        {
            _logger.LogInformation("Retrieving recurring transaction with ID {TransactionId}", transactionId);

            if (string.IsNullOrWhiteSpace(transactionId))
            {
                _logger.LogError("GetRecurringTransactionById failed: Transaction ID is required");
                throw new ArgumentException("Transaction ID is required.", nameof(transactionId));
            }

            var transaction = _recurringTransactionRepository.GetById(transactionId);
            if (transaction == null)
            {
                _logger.LogWarning("Recurring transaction with ID {TransactionId} not found", transactionId);
            }
            else
            {
                _logger.LogInformation("Retrieved recurring transaction with ID {TransactionId}", transactionId);
                _auditService.LogAction(transaction.UserId, "GetRecurringTransactionById", $"Retrieved recurring transaction with ID {transactionId}");
            }
            return transaction;
        }

        public void UpdateRecurringTransaction(RecurringTransaction transaction)
        {
            _logger.LogInformation("Updating recurring transaction with ID {TransactionId}", transaction?.TransactionId);

            if (transaction == null)
            {
                _logger.LogError("UpdateRecurringTransaction failed: Transaction cannot be null");
                throw new ArgumentNullException(nameof(transaction));
            }

            ValidateUser(transaction.UserId);

            var existingTransaction = _recurringTransactionRepository.GetById(transaction.TransactionId);
            if (existingTransaction == null)
            {
                _logger.LogError("UpdateRecurringTransaction failed: Recurring transaction with ID {TransactionId} not found for user {UserId}", transaction.TransactionId, transaction.UserId);
                throw new KeyNotFoundException($"Recurring transaction with ID {transaction.TransactionId} not found.");
            }

            _recurringTransactionRepository.Update(transaction);
            _auditService.LogAction(transaction.UserId, "UpdateRecurringTransaction", $"Recurring transaction updated with ID {transaction.TransactionId}");
            _logger.LogInformation("Recurring transaction with ID {TransactionId} updated", transaction.TransactionId);
        }

        public void DeleteRecurringTransaction(string transactionId)
        {
            _logger.LogInformation("Deleting recurring transaction with ID {TransactionId}", transactionId);

            if (string.IsNullOrWhiteSpace(transactionId))
            {
                _logger.LogError("DeleteRecurringTransaction failed: Transaction ID is required");
                throw new ArgumentException("Transaction ID is required.", nameof(transactionId));
            }

            var transaction = _recurringTransactionRepository.GetById(transactionId);
            if (transaction == null)
            {
                _logger.LogError("DeleteRecurringTransaction failed: Recurring transaction with ID {TransactionId} not found", transactionId);
                throw new KeyNotFoundException($"Recurring transaction with ID {transactionId} not found.");
            }

            ValidateUser(transaction.UserId);

            _recurringTransactionRepository.Delete(transaction);
            _auditService.LogAction(transaction.UserId, "DeleteRecurringTransaction", $"Recurring transaction deleted with ID {transactionId}");
            _logger.LogInformation("Recurring transaction with ID {TransactionId} deleted", transactionId);
        }
    }
}