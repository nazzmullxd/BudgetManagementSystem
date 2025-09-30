using Database.Model;

namespace Business.Services
{
    public interface IBudgetGoalService
    {
        void CreateGoal(BudgetGoal goal);
        List<BudgetGoal>? GetGoalsByUserId(string userId);
        BudgetGoal? GetGoalById(string goalId);
        void UpdateGoal(BudgetGoal goal);
        void DeleteGoal(string goalId);
    }
}
