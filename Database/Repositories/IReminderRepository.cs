using Database.Model;
using System.Collections.Generic;

namespace Database.Repositories
{
    public interface IReminderRepository
    {
        void Add(Reminder reminder);
        List<Reminder>? GetByUserId(string userId);
        Reminder? GetById(string reminderId);
        void Update(Reminder reminder);
        void Delete(Reminder reminder);
    }
}