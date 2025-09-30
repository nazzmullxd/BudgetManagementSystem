using Database.Model;

namespace Business.Services
{
    public interface IAlertService
    {
        void CreateAlert(BudgetAlerts alert);
        List<BudgetAlerts>? GetAlertsByUserId(string userId);
        BudgetAlerts? GetAlertById(string alertId);
        void UpdateAlert(BudgetAlerts alert);
        void DeleteAlert(string alertId);
    }
}