using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
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

        public List<AuditLog>? GetByUserId(string userId)
        {
            return _context.AuditLogs
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public List<AuditLog>? GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.AuditLogs
                .Include(a => a.User)
                .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
                .ToList();
        }
    }
}