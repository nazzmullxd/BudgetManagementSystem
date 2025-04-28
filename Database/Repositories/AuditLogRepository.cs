using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly BudgetManagementContext _context;

        public AuditLogRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            _context.SaveChanges();
        }

        public List<AuditLog> GetByUserId(string userId)
        {
            return _context.AuditLogs
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToList();
        }
    }
}