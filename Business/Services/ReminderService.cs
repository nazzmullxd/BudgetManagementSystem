using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ReminderService : BaseService, IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(
            IReminderRepository reminderRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<ReminderService> logger)
            : base(userRepository, auditService)
        {
            _reminderRepository = reminderRepository;
            _logger = logger;
        }

        public void CreateReminder(string userId, string description, DateTime dueDate)
        {
            _logger.LogInformation("Creating reminder for user {UserId}: {Description}", userId, description);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("CreateReminder failed: Description is required for user {UserId}", userId);
                throw new ArgumentException("Description is required.");
            }

            if (dueDate < DateTime.Now)
            {
                _logger.LogError("CreateReminder failed: Due date cannot be in the past for user {UserId}", userId);
                throw new ArgumentException("Due date cannot be in the past.");
            }

            var reminder = new Reminder
            {
                UserId = userId,
                Description = description,
                DueDate = dueDate,
                IsSent = false
            };

            _reminderRepository.Add(reminder);
            LogAction(_logger, userId, "Reminder Created", $"Reminder {description} created with due date {dueDate}");
        }

        public void UpdateReminder(string reminderId, string description, DateTime dueDate)
        {
            _logger.LogInformation("Updating reminder {ReminderId}", reminderId);

            if (string.IsNullOrWhiteSpace(reminderId))
            {
                _logger.LogError("UpdateReminder failed: Reminder ID is required.");
                throw new ArgumentException("Reminder ID is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("UpdateReminder failed: Description is required.");
                throw new ArgumentException("Description is required.");
            }

            if (dueDate < DateTime.Now)
            {
                _logger.LogError("UpdateReminder failed: Due date cannot be in the past.");
                throw new ArgumentException("Due date cannot be in the past.");
            }

            var reminder = _reminderRepository.GetById(reminderId);
            if (reminder == null)
            {
                _logger.LogError("UpdateReminder failed: Reminder {ReminderId} not found.", reminderId);
                throw new ArgumentException("Reminder not found.");
            }

            reminder.Description = description;
            reminder.DueDate = dueDate;

            _reminderRepository.Update(reminder);
            LogAction(_logger, reminder.UserId, "Reminder Updated", $"Reminder {reminderId} updated");
        }

        public void DeleteReminder(string reminderId)
        {
            _logger.LogInformation("Deleting reminder {ReminderId}", reminderId);

            if (string.IsNullOrWhiteSpace(reminderId))
            {
                _logger.LogError("DeleteReminder failed: Reminder ID is required.");
                throw new ArgumentException("Reminder ID is required.");
            }

            var reminder = _reminderRepository.GetById(reminderId);
            if (reminder == null)
            {
                _logger.LogError("DeleteReminder failed: Reminder {ReminderId} not found.", reminderId);
                throw new ArgumentException("Reminder not found.");
            }

            _reminderRepository.Delete(reminder);
            LogAction(_logger, reminder.UserId, "Reminder Deleted", $"Reminder {reminderId} deleted");
        }

        public IEnumerable<Reminder> GetRemindersForUser(string userId)
        {
            _logger.LogInformation("Retrieving all reminders for user {UserId}", userId);

            ValidateUser(userId);

            var reminders = _reminderRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} reminders for user {UserId}", reminders?.Count() ?? 0, userId);
            return reminders ?? new List<Reminder>();
        }

        public IEnumerable<Reminder> GetUpcomingReminders(string userId, DateTime? upToDate = null)
        {
            _logger.LogInformation("Retrieving upcoming reminders for user {UserId}", userId);

            ValidateUser(userId);

            var targetDate = upToDate ?? DateTime.Now.AddDays(7); // Default to next 7 days
            var reminders = _reminderRepository.GetUpcomingReminders(userId, targetDate);
            _logger.LogInformation("Retrieved {Count} upcoming reminders for user {UserId}", reminders?.Count() ?? 0, userId);
            return reminders ?? new List<Reminder>();
        }

        public void MarkReminderAsSent(string reminderId)
        {
            _logger.LogInformation("Marking reminder {ReminderId} as sent", reminderId);

            if (string.IsNullOrWhiteSpace(reminderId))
            {
                _logger.LogError("MarkReminderAsSent failed: Reminder ID is required.");
                throw new ArgumentException("Reminder ID is required.");
            }

            var reminder = _reminderRepository.GetById(reminderId);

            if (reminder == null)
            {
                _logger.LogError("MarkReminderAsSent failed: Reminder {ReminderId} not found.", reminderId);
                throw new ArgumentException("Reminder not found.");
            }

            reminder.IsSent = true;
            _reminderRepository.Update(reminder);
            LogAction(_logger, reminder.UserId, "Reminder Marked As Sent", $"Reminder {reminderId} marked as sent");
        }
    }
}