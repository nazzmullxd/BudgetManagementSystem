using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class DuesManagementService : BaseService, IDuesManagementService
    {
        private readonly IDuesManagementRepository _duesManagementRepository;
        private readonly ILogger<DuesManagementService> _logger;

        public DuesManagementService(
            IDuesManagementRepository duesManagementRepository,
            IUserRepository userRepository,
            ILogger<DuesManagementService> logger)
            : base(userRepository)
        {
            _duesManagementRepository = duesManagementRepository;
            _logger = logger;
        }

        public void CreateDue(DuesManagement due)
        {
            _logger.LogInformation("Creating due for user {UserId}", due?.UserId);

            if (due == null)
            {
                _logger.LogError("CreateDue failed: Due cannot be null");
                throw new ArgumentNullException(nameof(due));
            }

            ValidateUser(due.UserId);

            _duesManagementRepository.Add(due);
            _logger.LogInformation("Due created for user {UserId} with ID {DueId}", due.UserId, due.DueId);
        }

        public List<DuesManagement>? GetDuesByUserId(string userId)
        {
            _logger.LogInformation("Retrieving dues for user {UserId}", userId);

            ValidateUser(userId);

            var dues = _duesManagementRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} dues for user {UserId}", dues?.Count ?? 0, userId);
            return dues;
        }

        public DuesManagement? GetDueById(string dueId)
        {
            _logger.LogInformation("Retrieving due with ID {DueId}", dueId);

            if (string.IsNullOrWhiteSpace(dueId))
            {
                _logger.LogError("GetDueById failed: Due ID is required");
                throw new ArgumentException("Due ID is required.", nameof(dueId));
            }

            var due = _duesManagementRepository.GetById(dueId);
            if (due == null)
            {
                _logger.LogWarning("Due with ID {DueId} not found", dueId);
            }
            else
            {
                _logger.LogInformation("Retrieved due with ID {DueId}", dueId);
            }
            return due;
        }

        public void UpdateDue(DuesManagement due)
        {
            _logger.LogInformation("Updating due with ID {DueId}", due?.DueId);

            if (due == null)
            {
                _logger.LogError("UpdateDue failed: Due cannot be null");
                throw new ArgumentNullException(nameof(due));
            }

            ValidateUser(due.UserId);

            var existingDue = _duesManagementRepository.GetById(due.DueId);
            if (existingDue == null)
            {
                _logger.LogError("UpdateDue failed: Due with ID {DueId} not found", due.DueId);
                throw new KeyNotFoundException($"Due with ID {due.DueId} not found.");
            }

            _duesManagementRepository.Update(due);
            _logger.LogInformation("Due with ID {DueId} updated", due.DueId);
        }

        public void DeleteDue(string dueId)
        {
            _logger.LogInformation("Deleting due with ID {DueId}", dueId);

            if (string.IsNullOrWhiteSpace(dueId))
            {
                _logger.LogError("DeleteDue failed: Due ID is required");
                throw new ArgumentException("Due ID is required.", nameof(dueId));
            }

            var due = _duesManagementRepository.GetById(dueId);
            if (due == null)
            {
                _logger.LogError("DeleteDue failed: Due with ID {DueId} not found", dueId);
                throw new KeyNotFoundException($"Due with ID {dueId} not found.");
            }

            ValidateUser(due.UserId);

            _duesManagementRepository.Delete(due);
            _logger.LogInformation("Due with ID {DueId} deleted", dueId);
        }
    }
}
