namespace Business.Services
{
    public interface IBudgetService
    {
        void SetBudgetGoal(string userId, string categoryId, decimal amount, DateTime startDate, DateTime endDate);
        bool CheckBudgetExceeded(string userId, string categoryId, DateTime startDate, DateTime endDate);
        decimal GetTotalExpensesInCategory(string userId, string categoryId, DateTime startDate, DateTime endDate);
        void CheckAndCreateBudgetAlert(string userId, string categoryId, DateTime startDate, DateTime endDate);
    }
}