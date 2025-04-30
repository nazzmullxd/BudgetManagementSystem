using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Database.Repositories;


namespace Database.Repositories
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

        public List<ExpenseCategory>? GetByUserId(string userId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .Where(ec => ec.UserId == userId)
                .ToList();
        }

        public ExpenseCategory? GetById(string categoryId)
        {
            return _context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .Include(ec => ec.RecurringTransactions)
                .Include(ec => ec.BudgetGoals)
                .FirstOrDefault(ec => ec.ExpenseCategoryId == categoryId);
        }

        public void Update(ExpenseCategory category)
        {
            _context.ExpenseCategories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(ExpenseCategory category)
        {
            _context.ExpenseCategories.Remove(category);
            _context.SaveChanges();
        }
    }
}