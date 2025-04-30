using Database.Model;
using System.Collections.Generic;

namespace Database.Repositories
{
    public interface IRecurringTransactionRepository
    {
        void Add(RecurringTransaction transaction);
        List<RecurringTransaction>? GetByUserId(string userId);
        RecurringTransaction? GetById(string transactionId);
        void Update(RecurringTransaction transaction);
        void Delete(RecurringTransaction transaction);
    }
}