using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Database.Repositories
{
    public class RecurringTransactionRepository : IRecurringTransactionRepository
    {
        private readonly BudgetManagementContext _context;

        public RecurringTransactionRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(RecurringTransaction transaction)
        {
            if (transaction != null)
            {
                _context.RecurringTransactions.Add(transaction);
                _context.SaveChanges();
            }
        }

        public List<RecurringTransaction>? GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<RecurringTransaction>();

            return _context.RecurringTransactions
                .Include(rt => rt.User)
                .Include(rt => rt.Category)
                .Include(rt => rt.Currency)
                .Include(rt => rt.TransactionTags)
                    .ThenInclude(tt => tt.Tag)
                .Where(rt => rt.UserId == userId)
                .ToList();
        }

        public RecurringTransaction? GetById(string transactionId)
        {
            return _context.RecurringTransactions
                .Include(rt => rt.User)
                .Include(rt => rt.Category)
                .Include(rt => rt.Currency)
                .Include(rt => rt.TransactionTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefault(rt => rt.TransactionId == transactionId);
        }

        public void Update(RecurringTransaction transaction)
        {
            if (transaction != null)
            {
                _context.RecurringTransactions.Update(transaction);
                _context.SaveChanges();
            }
        }

        public void Delete(RecurringTransaction transaction)
        {
            if (transaction != null)
            {
                _context.RecurringTransactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }
}