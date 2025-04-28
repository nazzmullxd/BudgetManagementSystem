using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface ITransactionTagRepository
    {
        void Add(TransactionTag transactionTag);
        List<TransactionTag> GetByTagId(string tagId);
        List<TransactionTag> GetByExpenseId(string expenseId);
        List<TransactionTag> GetByIncomeId(string incomeId);
    }
}