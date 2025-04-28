using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IBudgetAlertsRepository
    {
        void Add(BudgetAlerts alert);
        List<BudgetAlerts> GetByUserId(string userId);
        BudgetAlerts GetById(string alertId);
        void Update(BudgetAlerts alert);
    }
}