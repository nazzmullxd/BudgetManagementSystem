using Database.Model;

namespace Database.Repositories
{
    public interface ITransactionTagRepository
    {
        void Add(TransactionTag transactionTag);
        List<TransactionTag>? GetByTagId(string tagId);
        List<TransactionTag>? GetByExpenseId(string expenseId);
        List<TransactionTag>? GetByIncomeId(string incomeId);
        TransactionTag? GetById(string transactionTagId);
        void Update(TransactionTag transactionTag);
        void Delete(TransactionTag transactionTag);
    }
}