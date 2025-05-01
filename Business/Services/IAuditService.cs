using Database.Model;

namespace Business.Services
{
    public interface IAuditService
    {
        void LogAction(string userId, string action, string details);
        List<AuditLog>? GetAuditLogsByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}