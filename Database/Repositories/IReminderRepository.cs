using Database.Model;
using System;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IReminderRepository
    {
        void Add(Reminder reminder);
        List<Reminder> GetByUserId(string userId);
        List<Reminder> GetUpcomingReminders(string userId, DateTime upcomingDate);
        void Update(Reminder reminder);
    }
}