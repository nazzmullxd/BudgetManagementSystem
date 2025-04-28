using Database.Model;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManagementSystem.Repositories
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
            _context.BudgetAlerts.Add(alert);
            _context.SaveChanges();
        }

        public List<BudgetAlerts> GetByUserId(string userId)
        {
            return _context.BudgetAlerts
                .Include(ba => ba.User)
                .Where(ba => ba.UserId == userId)
                .ToList();
        }

        public BudgetAlerts GetById(string alertId)
        {
            return _context.BudgetAlerts
                .Include(ba => ba.User)
                .FirstOrDefault(ba => ba.BudgetAlertsId == alertId);
        }

        public void Update(BudgetAlerts alert)
        {
            _context.BudgetAlerts.Update(alert);
            _context.SaveChanges();
        }
    }
}