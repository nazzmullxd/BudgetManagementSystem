using Database.Model;

namespace Database.Repositories
{
    public interface IReminderRepository
    {
        void Add(Reminder reminder);
        List<Reminder>? GetByUserId(string userId);
        Reminder? GetById(string reminderId);
        void Update(Reminder reminder);
        void Delete(Reminder reminder);
        List<Reminder> GetUpcomingReminders(string userId, DateTime endDate); // Added method
    }
}