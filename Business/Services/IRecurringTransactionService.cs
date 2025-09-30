using Database.Model;

namespace Business.Services
{
    public interface IRecurringTransactionService
    {
        void CreateRecurringTransaction(RecurringTransaction transaction);
        List<RecurringTransaction>? GetRecurringTransactionsByUserId(string userId);
        RecurringTransaction? GetRecurringTransactionById(string transactionId);
        void UpdateRecurringTransaction(RecurringTransaction transaction);
        void DeleteRecurringTransaction(string transactionId);
    }
}