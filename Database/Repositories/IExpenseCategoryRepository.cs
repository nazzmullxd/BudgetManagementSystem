using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IExpenseCategoryRepository
    {
        void Add(ExpenseCategory category);
        List<ExpenseCategory> GetByUserId(string userId);
        ExpenseCategory GetById(string categoryId);
    }
}