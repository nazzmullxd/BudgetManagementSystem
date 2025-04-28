using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly BudgetManagementContext _context;

        public ExpenseCategoryRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(ExpenseCategory category)
        {
            _context.ExpenseCategories.Add(category);
            _context.SaveChanges();
        }

        public List<ExpenseCategory> GetByUserId(string userId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .Where(ec => ec.UserId == userId)
                .ToList();
        }

        public ExpenseCategory GetById(string categoryId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .FirstOrDefault(ec => ec.ExpenseCategoryId == categoryId);
        }
    }
}