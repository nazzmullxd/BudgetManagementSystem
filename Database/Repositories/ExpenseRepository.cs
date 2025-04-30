using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Database.Repositories;


namespace Database.Repositories
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

        public List<TrackExpense>? GetByUserId(string userId)
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

        public List<TrackExpense>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
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

        public void Update(TrackExpense expense)
        {
            _context.TrackExpenses.Update(expense);
            _context.SaveChanges();
        }

        public void Delete(TrackExpense expense)
        {
            _context.TrackExpenses.Remove(expense);
            _context.SaveChanges();
        }
    }
}