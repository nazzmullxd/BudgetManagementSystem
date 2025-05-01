using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class TransactionTagRepository : ITransactionTagRepository
    {
        private readonly BudgetManagementContext _context;

        public TransactionTagRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(TransactionTag transactionTag)
        {
            _context.TransactionTags.Add(transactionTag);
            _context.SaveChanges();
        }

        public List<TransactionTag>? GetByTagId(string tagId)
        {
            return _context.TransactionTags
                .Include(tt => tt.Tag)
                .Include(tt => tt.Expense)
                .Include(tt => tt.Income)
                .Where(tt => tt.TagId == tagId)
                .ToList();
        }

        public List<TransactionTag>? GetByExpenseId(string expenseId)
        {
            return _context.TransactionTags
                .Include(tt => tt.Tag)
                .Include(tt => tt.Expense)
                .Include(tt => tt.Income)
                .Where(tt => tt.TrackExpenseId == expenseId)
                .ToList();
        }

        public List<TransactionTag>? GetByIncomeId(string incomeId)
        {
            return _context.TransactionTags
                .Include(tt => tt.Tag)
                .Include(tt => tt.Expense)
                .Include(tt => tt.Income)
                .Where(tt => tt.TrackIncomeId == incomeId)
                .ToList();
        }

        public TransactionTag? GetById(string transactionTagId)
        {
            return _context.TransactionTags
                .Include(tt => tt.Tag)
                .Include(tt => tt.Expense)
                .Include(tt => tt.Income)
                .FirstOrDefault(tt => tt.TransactionTagId == transactionTagId);
        }

        public void Update(TransactionTag transactionTag)
        {
            _context.TransactionTags.Update(transactionTag);
            _context.SaveChanges();
        }

        public void Delete(TransactionTag transactionTag)
        {
            _context.TransactionTags.Remove(transactionTag);
            _context.SaveChanges();
        }
    }
}