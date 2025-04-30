using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

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
                IsSent = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _reminderRepository.Add(reminder);
            LogAction(_logger, userId, "Reminder Created", $"Reminder {description} created with due date {dueDate}");
        }

        public List<Reminder> GetUpcomingReminders(string userId, DateTime upcomingDate)
        {
            _logger.LogInformation("Retrieving upcoming reminders for user {UserId}", userId);

            ValidateUser(userId);

            var reminders = _reminderRepository.GetUpcomingReminders(userId, upcomingDate);
            _logger.LogInformation("Retrieved {Count} upcoming reminders for user {UserId}", reminders.Count, userId);
            return reminders;
        }

        public void MarkReminderAsSent(string reminderId)
        {
            _logger.LogInformation("Marking reminder {ReminderId} as sent", reminderId);

            if (string.IsNullOrWhiteSpace(reminderId))
            {
                _logger.LogError("MarkReminderAsSent failed: Reminder ID is required.");
                throw new ArgumentException("Reminder ID is required.");
            }

            var reminder = _reminderRepository.GetByUserId(null)
                .FirstOrDefault(r => r.ReminderId == reminderId);

            if (reminder == null)
            {
                _logger.LogError("MarkReminderAsSent failed: Reminder {ReminderId} not found.", reminderId);
                throw new ArgumentException("Reminder not found.");
            }

            reminder.IsSent = true;
            reminder.UpdatedAt = DateTime.Now;
            _reminderRepository.Update(reminder);
            LogAction(_logger, reminder.UserId, "Reminder Marked As Sent", $"Reminder {reminderId} marked as sent");
        }
    }
}