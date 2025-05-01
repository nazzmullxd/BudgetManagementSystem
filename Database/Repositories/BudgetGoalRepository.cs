using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;


namespace Database.Repositories
{
    public class BudgetGoalRepository : IBudgetGoalRepository
    {
        private readonly BudgetManagementContext _context;

        public BudgetGoalRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(BudgetGoal goal)
        {
            _context.BudgetGoals.Add(goal);
            _context.SaveChanges();
        }

        public List<BudgetGoal>? GetByUserId(string userId)
        {
            return _context.BudgetGoals
                .Include(bg => bg.User)
                .Include(bg => bg.Category)
                .Where(bg => bg.UserId == userId)
                .ToList();
        }

        public BudgetGoal? GetById(string goalId)
        {
            return _context.BudgetGoals
                .Include(bg => bg.User)
                .Include(bg => bg.Category)
                .FirstOrDefault(bg => bg.BudgetGoalId == goalId);
        }

        public void Update(BudgetGoal goal)
        {
            _context.BudgetGoals.Update(goal);
            _context.SaveChanges();
        }
    }
}