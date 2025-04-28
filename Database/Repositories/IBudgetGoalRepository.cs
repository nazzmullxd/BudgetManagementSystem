using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IBudgetGoalRepository
    {
        void Add(BudgetGoal goal);
        List<BudgetGoal> GetByUserId(string userId);
        BudgetGoal GetById(string goalId);
        void Update(BudgetGoal goal);
    }
}