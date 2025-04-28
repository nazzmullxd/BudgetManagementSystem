using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
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
            _context.RecurringTransactions.Add(transaction);
            _context.SaveChanges();
        }

        public List<RecurringTransaction> GetByUserId(string userId)
        {
            return _context.RecurringTransactions
                .Include(rt => rt.User)
                .Include(rt => rt.Category)
                .Where(rt => rt.UserId == userId)
                .ToList();
        }

        public RecurringTransaction GetById(string transactionId)
        {
            return _context.RecurringTransactions
                .Include(rt => rt.User)
                .Include(rt => rt.Category)
                .FirstOrDefault(rt => rt.RecurringTransactionId == transactionId);
        }
    }
}