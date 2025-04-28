using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IAuditLogRepository
    {
        void Add(AuditLog auditLog);
        List<AuditLog> GetByUserId(string userId);
    }
}