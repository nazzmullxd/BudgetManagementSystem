using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class TransactionTagService : BaseService, ITransactionTagService
    {
        private readonly ITransactionTagRepository _transactionTagRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<TransactionTagService> _logger;

        public TransactionTagService(
            ITransactionTagRepository transactionTagRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<TransactionTagService> logger)
            : base(userRepository)
        {
            _transactionTagRepository = transactionTagRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateTransactionTag(TransactionTag transactionTag)
        {
            _logger.LogInformation("Creating transaction tag with TagId {TagId}", transactionTag?.TagId);

            if (transactionTag == null)
            {
                _logger.LogError("CreateTransactionTag failed: TransactionTag cannot be null");
                throw new ArgumentNullException(nameof(transactionTag));
            }

            if (string.IsNullOrWhiteSpace(transactionTag.TagId))
            {
                _logger.LogError("CreateTransactionTag failed: TagId cannot be null or empty");
                throw new ArgumentException("TagId cannot be null or empty.", nameof(transactionTag.TagId));
            }

            // Validate that either TrackExpenseId or TrackIncomeId is set, but not both
            if (string.IsNullOrWhiteSpace(transactionTag.TrackExpenseId) && string.IsNullOrWhiteSpace(transactionTag.TrackIncomeId))
            {
                _logger.LogError("CreateTransactionTag failed: Either TrackExpenseId or TrackIncomeId must be set");
                throw new ArgumentException("Either TrackExpenseId or TrackIncomeId must be set.");
            }

            if (!string.IsNullOrWhiteSpace(transactionTag.TrackExpenseId) && !string.IsNullOrWhiteSpace(transactionTag.TrackIncomeId))
            {
                _logger.LogError("CreateTransactionTag failed: TrackExpenseId and TrackIncomeId cannot both be set");
                throw new ArgumentException("TrackExpenseId and TrackIncomeId cannot both be set.");
            }

            // Determine the userId based on the associated expense or income
            string userId = GetUserIdForTransactionTag(transactionTag);
            ValidateUser(userId);

            _transactionTagRepository.Add(transactionTag);
            _auditService.LogAction(userId, "CreateTransactionTag", $"Transaction tag created with ID {transactionTag.TransactionTagId}");
            _logger.LogInformation("Transaction tag created with ID {TransactionTagId}", transactionTag.TransactionTagId);
        }

        public List<TransactionTag>? GetTransactionTagsByTagId(string tagId)
        {
            _logger.LogInformation("Retrieving transaction tags for tag {TagId}", tagId);

            if (string.IsNullOrWhiteSpace(tagId))
            {
                _logger.LogError("GetTransactionTagsByTagId failed: Tag ID is required");
                throw new ArgumentException("Tag ID is required.", nameof(tagId));
            }

            var transactionTags = _transactionTagRepository.GetByTagId(tagId);
            if (transactionTags != null && transactionTags.Any())
            {
                string userId = GetUserIdForTransactionTag(transactionTags.First());
                _auditService.LogAction(userId, "GetTransactionTagsByTagId", $"Retrieved {transactionTags.Count} transaction tags for tag {tagId}");
            }
            _logger.LogInformation("Retrieved {Count} transaction tags for tag {TagId}", transactionTags?.Count ?? 0, tagId);
            return transactionTags;
        }

        public List<TransactionTag>? GetTransactionTagsByExpenseId(string expenseId)
        {
            _logger.LogInformation("Retrieving transaction tags for expense {ExpenseId}", expenseId);

            if (string.IsNullOrWhiteSpace(expenseId))
            {
                _logger.LogError("GetTransactionTagsByExpenseId failed: Expense ID is required");
                throw new ArgumentException("Expense ID is required.", nameof(expenseId));
            }

            var transactionTags = _transactionTagRepository.GetByExpenseId(expenseId);
            if (transactionTags != null && transactionTags.Any())
            {
                string userId = GetUserIdForTransactionTag(transactionTags.First());
                _auditService.LogAction(userId, "GetTransactionTagsByExpenseId", $"Retrieved {transactionTags.Count} transaction tags for expense {expenseId}");
            }
            _logger.LogInformation("Retrieved {Count} transaction tags for expense {ExpenseId}", transactionTags?.Count ?? 0, expenseId);
            return transactionTags;
        }

        public List<TransactionTag>? GetTransactionTagsByIncomeId(string incomeId)
        {
            _logger.LogInformation("Retrieving transaction tags for income {IncomeId}", incomeId);

            if (string.IsNullOrWhiteSpace(incomeId))
            {
                _logger.LogError("GetTransactionTagsByIncomeId failed: Income ID is required");
                throw new ArgumentException("Income ID is required.", nameof(incomeId));
            }

            var transactionTags = _transactionTagRepository.GetByIncomeId(incomeId);
            if (transactionTags != null && transactionTags.Any())
            {
                string userId = GetUserIdForTransactionTag(transactionTags.First());
                _auditService.LogAction(userId, "GetTransactionTagsByIncomeId", $"Retrieved {transactionTags.Count} transaction tags for income {incomeId}");
            }
            _logger.LogInformation("Retrieved {Count} transaction tags for income {IncomeId}", transactionTags?.Count ?? 0, incomeId);
            return transactionTags;
        }

        public void UpdateTransactionTag(TransactionTag transactionTag)
        {
            _logger.LogInformation("Updating transaction tag with ID {TransactionTagId}", transactionTag?.TransactionTagId);

            if (transactionTag == null)
            {
                _logger.LogError("UpdateTransactionTag failed: TransactionTag cannot be null");
                throw new ArgumentNullException(nameof(transactionTag));
            }

            string userId = GetUserIdForTransactionTag(transactionTag);
            ValidateUser(userId);

            var existingTransactionTag = _transactionTagRepository.GetById(transactionTag.TransactionTagId);
            if (existingTransactionTag == null)
            {
                _logger.LogError("UpdateTransactionTag failed: Transaction tag with ID {TransactionTagId} not found", transactionTag.TransactionTagId);
                throw new KeyNotFoundException($"Transaction tag with ID {transactionTag.TransactionTagId} not found.");
            }

            _transactionTagRepository.Update(transactionTag);
            _auditService.LogAction(userId, "UpdateTransactionTag", $"Transaction tag updated with ID {transactionTag.TransactionTagId}");
            _logger.LogInformation("Transaction tag with ID {TransactionTagId} updated", transactionTag.TransactionTagId);
        }

        public void DeleteTransactionTag(string transactionTagId)
        {
            _logger.LogInformation("Deleting transaction tag with ID {TransactionTagId}", transactionTagId);

            if (string.IsNullOrWhiteSpace(transactionTagId))
            {
                _logger.LogError("DeleteTransactionTag failed: Transaction tag ID is required");
                throw new ArgumentException("Transaction tag ID is required.", nameof(transactionTagId));
            }

            var transactionTag = _transactionTagRepository.GetById(transactionTagId);
            if (transactionTag == null)
            {
                _logger.LogError("DeleteTransactionTag failed: Transaction tag with ID {TransactionTagId} not found", transactionTagId);
                throw new KeyNotFoundException($"Transaction tag with ID {transactionTagId} not found.");
            }

            string userId = GetUserIdForTransactionTag(transactionTag);
            ValidateUser(userId);

            _transactionTagRepository.Delete(transactionTag);
            _auditService.LogAction(userId, "DeleteTransactionTag", $"Transaction tag deleted with ID {transactionTagId}");
            _logger.LogInformation("Transaction tag with ID {TransactionTagId} deleted", transactionTagId);
        }

        private string GetUserIdForTransactionTag(TransactionTag transactionTag)
        {
            if (!string.IsNullOrWhiteSpace(transactionTag.TrackExpenseId))
            {
                // In a real app, you'd query the TrackExpense to get the UserId
                // For now, assume the UserId is accessible via Expense (simplified)
                return transactionTag.Expense?.UserId ?? throw new InvalidOperationException("UserId cannot be determined for the transaction tag.");
            }
            else if (!string.IsNullOrWhiteSpace(transactionTag.TrackIncomeId))
            {
                return transactionTag.Income?.UserId ?? throw new InvalidOperationException("UserId cannot be determined for the transaction tag.");
            }
            throw new InvalidOperationException("Transaction tag must be associated with either an expense or an income.");
        }
    }
}