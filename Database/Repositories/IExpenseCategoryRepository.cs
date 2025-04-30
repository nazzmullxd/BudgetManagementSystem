using Database.Model;
using System.Collections.Generic;

namespace Database.Repositories
{
    public interface IExpenseCategoryRepository
    {
        void Add(ExpenseCategory category);
        List<ExpenseCategory>? GetByUserId(string userId);
        ExpenseCategory? GetById(string categoryId);
        void Update(ExpenseCategory category);
        void Delete(ExpenseCategory category);
    }
}