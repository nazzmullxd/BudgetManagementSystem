using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
{
    public class DuesManagementRepository : IDuesManagementRepository
    {
        private readonly BudgetManagementContext _context;

        public DuesManagementRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(DuesManagement due)
        {
            _context.DuesManagements.Add(due);
            _context.SaveChanges();
        }

        public List<DuesManagement> GetByUserId(string userId)
        {
            return _context.DuesManagements
                .Include(d => d.User)
                .Where(d => d.UserId == userId)
                .ToList();
        }

        public DuesManagement GetById(string dueId)
        {
            return _context.DuesManagements
                .Include(d => d.User)
                .FirstOrDefault(d => d.DueId == dueId);
        }

        public void Update(DuesManagement due)
        {
            _context.DuesManagements.Update(due);
            _context.SaveChanges();
        }
    }
}