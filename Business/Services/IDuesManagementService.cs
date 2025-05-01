using Database.Model;

namespace Business.Services
{
    public interface IDuesManagementService
    {
        void CreateDue(DuesManagement due);
        List<DuesManagement>? GetDuesByUserId(string userId);
        DuesManagement? GetDueById(string dueId);
        void UpdateDue(DuesManagement due);
        void DeleteDue(string dueId);
    }
}