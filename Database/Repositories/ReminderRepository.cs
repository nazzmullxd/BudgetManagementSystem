using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly BudgetManagementContext _context;

        public ReminderRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            _context.SaveChanges();
        }

        public List<Reminder>? GetByUserId(string userId)
        {
            return _context.Reminders
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public Reminder? GetById(string reminderId)
        {
            return _context.Reminders
                .Include(r => r.User)
                .FirstOrDefault(r => r.ReminderId == reminderId);
        }

        public void Update(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            _context.SaveChanges();
        }

        public void Delete(Reminder reminder)
        {
            _context.Reminders.Remove(reminder);
            _context.SaveChanges();
        }
        public List<Reminder> GetUpcomingReminders(string userId, DateTime endDate)
        {
            return _context.Reminders
                .Include(r => r.User)
                .Where(r => r.UserId == userId && r.DueDate <= endDate)
                .ToList();
        }
    }
}