using Business.Interface;
using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class BudgetGoalService : BaseService, IBudgetGoalService
    {
        private readonly IBudgetGoalRepository _budgetGoalRepository;
        private readonly ILogger<BudgetGoalService> _logger;

        public BudgetGoalService(
            IBudgetGoalRepository budgetGoalRepository,
            IUserRepository userRepository,
            ILogger<BudgetGoalService> logger)
            : base(userRepository)
        {
            _budgetGoalRepository = budgetGoalRepository;
            _logger = logger;
        }

        public void CreateGoal(BudgetGoal goal)
        {
            _logger.LogInformation("Creating budget goal for user {UserId}", goal?.UserId);

            if (goal == null)
            {
                _logger.LogError("CreateGoal failed: Goal cannot be null");
                throw new ArgumentNullException(nameof(goal));
            }

            ValidateUser(goal.UserId);

            _budgetGoalRepository.Add(goal);
            _logger.LogInformation("Budget goal created for user {UserId} with ID {GoalId}", goal.UserId, goal.BudgetGoalId);
        }

        public List<BudgetGoal>? GetGoalsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving budget goals for user {UserId}", userId);

            ValidateUser(userId);

            var goals = _budgetGoalRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} budget goals for user {UserId}", goals?.Count ?? 0, userId);
            return goals;
        }

        public BudgetGoal? GetGoalById(string goalId)
        {
            _logger.LogInformation("Retrieving budget goal with ID {GoalId}", goalId);

            if (string.IsNullOrWhiteSpace(goalId))
            {
                _logger.LogError("GetGoalById failed: Goal ID is required");
                throw new ArgumentException("Goal ID is required.", nameof(goalId));
            }

            var goal = _budgetGoalRepository.GetById(goalId);
            if (goal == null)
            {
                _logger.LogWarning("Budget goal with ID {GoalId} not found", goalId);
            }
            else
            {
                _logger.LogInformation("Retrieved budget goal with ID {GoalId}", goalId);
            }
            return goal;
        }

        public void UpdateGoal(BudgetGoal goal)
        {
            _logger.LogInformation("Updating budget goal with ID {GoalId}", goal?.BudgetGoalId);

            if (goal == null)
            {
                _logger.LogError("UpdateGoal failed: Goal cannot be null");
                throw new ArgumentNullException(nameof(goal));
            }

            ValidateUser(goal.UserId);

            var existingGoal = _budgetGoalRepository.GetById(goal.BudgetGoalId);
            if (existingGoal == null)
            {
                _logger.LogError("UpdateGoal failed: Budget goal with ID {GoalId} not found", goal.BudgetGoalId);
                throw new KeyNotFoundException($"Budget goal with ID {goal.BudgetGoalId} not found.");
            }

            _budgetGoalRepository.Update(goal);
            _logger.LogInformation("Budget goal with ID {GoalId} updated", goal.BudgetGoalId);
        }

        public void DeleteGoal(string goalId)
        {
            _logger.LogInformation("Deleting budget goal with ID {GoalId}", goalId);

            if (string.IsNullOrWhiteSpace(goalId))
            {
                _logger.LogError("DeleteGoal failed: Goal ID is required");
                throw new ArgumentException("Goal ID is required.", nameof(goalId));
            }

            var goal = _budgetGoalRepository.GetById(goalId);
            if (goal == null)
            {
                _logger.LogError("DeleteGoal failed: Budget goal with ID {GoalId} not found", goalId);
                throw new KeyNotFoundException($"Budget goal with ID {goalId} not found.");
            }

            ValidateUser(goal.UserId);

            _budgetGoalRepository.Delete(goal);
            _logger.LogInformation("Budget goal with ID {GoalId} deleted", goalId);
        }
    }
}