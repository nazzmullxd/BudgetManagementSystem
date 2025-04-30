using Database.Model;
using System;
using System.Collections.Generic;

namespace Database.Repositories
{
    public interface IAuditLogRepository
    {
        void Add(AuditLog auditLog);
        List<AuditLog>? GetByUserId(string userId);
        List<AuditLog>? GetByDateRange(DateTime startDate, DateTime endDate);
    }
}