using Database.Model;  // For User, TrackExpense, etc.
using Database.Context;  // Change this from BudgetManagementSystem.DB_Context
using Microsoft.EntityFrameworkCore;  // Add this for DbContext
using System.Linq;

namespace BudgetManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BudgetManagementContext _context;

        public UserRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetById(string userId)
        {
            return _context.Users
                .Include(u => u.Expenses)
                .Include(u => u.Incomes)
                .Include(u => u.Categories)
                .Include(u => u.Dues)
                .Include(u => u.BudgetAlerts)
                .Include(u => u.Reminders)
                .Include(u => u.RecurringTransactions)
                .Include(u => u.Tags)
                .Include(u => u.AuditLogs)
                .Include(u => u.BudgetGoals)
                .Include(u => u.PreferredCurrency)
                .FirstOrDefault(u => u.UserId == userId);
        }

        public User GetByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Expenses)
                .Include(u => u.Incomes)
                .Include(u => u.Categories)
                .Include(u => u.Dues)
                .Include(u => u.BudgetAlerts)
                .Include(u => u.Reminders)
                .Include(u => u.RecurringTransactions)
                .Include(u => u.Tags)
                .Include(u => u.AuditLogs)
                .Include(u => u.BudgetGoals)
                .Include(u => u.PreferredCurrency)
                .FirstOrDefault(u => u.Email == email);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}