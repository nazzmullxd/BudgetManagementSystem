using Database.Model;

namespace Business.Services
{
    public interface IReminderService
    {
        void CreateReminder(string userId, string description, DateTime dueDate);
        void UpdateReminder(string reminderId, string description, DateTime dueDate);
        void DeleteReminder(string reminderId);
        IEnumerable<Reminder> GetRemindersForUser(string userId);
        IEnumerable<Reminder> GetUpcomingReminders(string userId, DateTime? upToDate = null);
        void MarkReminderAsSent(string reminderId);
    }
}