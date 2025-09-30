using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
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

        public User? GetById(string userId)
        {
            return _context.Users
                .Include(u => u.TrackExpenses)
                .Include(u => u.TrackIncomes)
                .Include(u => u.BudgetGoals)
                .Include(u => u.ExpenseCategories)
                .Include(u => u.BudgetAlerts)
                .Include(u => u.Dues)
                .Include(u => u.RecurringTransactions)
                .Include(u => u.AuditLogs)
                .Include(u => u.Tags)
                .FirstOrDefault(u => u.UserId == userId);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .Include(u => u.TrackExpenses)
                .Include(u => u.TrackIncomes)
                .Include(u => u.BudgetGoals)
                .Include(u => u.ExpenseCategories)
                .Include(u => u.BudgetAlerts)
                .Include(u => u.Dues)
                .Include(u => u.RecurringTransactions)
                .Include(u => u.AuditLogs)
                .Include(u => u.Tags)
                .FirstOrDefault(u => u.Email == email);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}