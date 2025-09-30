using Database.Model;

namespace Business.Services
{
    public interface ITransactionTagService
    {
        void CreateTransactionTag(TransactionTag transactionTag);
        List<TransactionTag>? GetTransactionTagsByTagId(string tagId);
        List<TransactionTag>? GetTransactionTagsByExpenseId(string expenseId);
        List<TransactionTag>? GetTransactionTagsByIncomeId(string incomeId);
        void UpdateTransactionTag(TransactionTag transactionTag);
        void DeleteTransactionTag(string transactionTagId);
    }
}