using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BudgetManagementContext _context;

        public TagRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        public List<Tag> GetByUserId(string userId)
        {
            return _context.Tags
                .Include(t => t.TransactionTags)
                .ThenInclude(tt => tt.Expense)
                .Include(t => t.TransactionTags)
                .ThenInclude(tt => tt.Income)
                .Where(t => t.UserId == userId)
                .ToList();
        }

        public Tag GetById(string tagId)
        {
            return _context.Tags
                .Include(t => t.TransactionTags)
                .ThenInclude(tt => tt.Expense)
                .Include(t => t.TransactionTags)
                .ThenInclude(tt => tt.Income)
                .FirstOrDefault(t => t.TagId == tagId);
        }
    }
}