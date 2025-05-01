using Database.Model;

namespace Database.Repositories
{
    public interface IBudgetGoalRepository
    {
        void Add(BudgetGoal goal);
        List<BudgetGoal>? GetByUserId(string userId);
        BudgetGoal? GetById(string goalId);
        void Update(BudgetGoal goal);
    }
}