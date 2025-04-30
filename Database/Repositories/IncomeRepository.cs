using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly BudgetManagementContext _context;

        public IncomeRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(TrackIncome income)
        {
            _context.TrackIncomes.Add(income);
            _context.SaveChanges();
        }

        public List<TrackIncome>? GetByUserId(string userId)
        {
            return _context.TrackIncomes
                .Include(i => i.User)
                .Include(i => i.Currency)
                .Include(i => i.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(i => i.UserId == userId)
                .ToList();
        }

        public List<TrackIncome>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate)
        {
            return _context.TrackIncomes
                .Include(i => i.User)
                .Include(i => i.Currency)
                .Include(i => i.TransactionTags)
                .ThenInclude(tt => tt.Tag)
                .Where(i => i.UserId == userId && i.IncomeDate >= startDate && i.IncomeDate <= endDate)
                .ToList();
        }

        public void Update(TrackIncome income)
        {
            _context.TrackIncomes.Update(income);
            _context.SaveChanges();
        }

        public void Delete(TrackIncome income)
        {
            _context.TrackIncomes.Remove(income);
            _context.SaveChanges();
        }
    }
}