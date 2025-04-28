using Database.Model;  // For TrackExpense, User, etc.
using Database.Context;  // Change this
using Microsoft.EntityFrameworkCore;  // Add this
using System;
using System.Collections.Generic;
using System.Linq;
namespace BudgetManagementSystem.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly BudgetManagementContext _context;

        public ExpenseRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(TrackExpense expense)
        {
            _context.TrackExpenses.Add(expense);
            _context.SaveChanges();
        }

        public List<TrackExpense> GetByUserId(string userId)
        {
            return _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId)
                .ToList();
        }

        public List<TrackExpense> GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
        {
            return _context.TrackExpenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.Currency)
                .Include(e => e.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(e => e.UserId == userId && e.TransactionDate >= startDate && e.TransactionDate <= endDate)
                .ToList();
        }
    }
}