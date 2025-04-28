using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IRecurringTransactionRepository
    {
        void Add(RecurringTransaction transaction);
        List<RecurringTransaction> GetByUserId(string userId);
        RecurringTransaction GetById(string transactionId);
    }
}