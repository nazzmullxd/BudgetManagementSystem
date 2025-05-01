using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class BudgetAlertsRepository : IBudgetAlertsRepository
    {
        private readonly BudgetManagementContext _context;

        public BudgetAlertsRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(BudgetAlerts alert)
        {
            if (alert != null)
            {
                _context.BudgetAlerts.Add(alert);
                _context.SaveChanges();
            }
        }

        public List<BudgetAlerts> GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<BudgetAlerts>();

            return _context.BudgetAlerts
                .Include(ba => ba.User)
                .Where(ba => ba.UserId == userId)
                .ToList();
        }

        BudgetAlerts? IBudgetAlertsRepository.GetById(string alertId)
        {
            if (string.IsNullOrWhiteSpace(alertId))
                return null;

            return _context.BudgetAlerts
                .Include(ba => ba.User)
                .FirstOrDefault(ba => ba.BudgetAlertsId == alertId);
        }

        public void Update(BudgetAlerts alert)
        {
            if (alert != null)
            {
                _context.BudgetAlerts.Update(alert);
                _context.SaveChanges();
            }
        }
    }
}
