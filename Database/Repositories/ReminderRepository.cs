using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
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

        public List<Reminder> GetByUserId(string userId)
        {
            return _context.Reminders
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public List<Reminder> GetUpcomingReminders(string userId, DateTime upcomingDate)
        {
            return _context.Reminders
                .Include(r => r.User)
                .Where(r => r.UserId == userId && r.DueDate <= upcomingDate && !r.IsSent)
                .ToList();
        }

        public void Update(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            _context.SaveChanges();
        }
    }
}