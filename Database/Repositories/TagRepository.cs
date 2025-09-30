using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
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

        public List<Tag>? GetByUserId(string userId)
        {
            return _context.Tags
                .Include(t => t.TransactionTags)
                .Where(t => t.UserId == userId)
                .ToList();
        }

        public Tag? GetById(string tagId)
        {
            return _context.Tags
                .Include(t => t.TransactionTags)
                .FirstOrDefault(t => t.TagId == tagId);
        }

        public void Update(Tag tag)
        {
            _context.Tags.Update(tag);
            _context.SaveChanges();
        }

        public void Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }
    }
}