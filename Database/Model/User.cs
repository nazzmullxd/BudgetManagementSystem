using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public partial class User
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<TrackExpense> TrackExpenses { get; set; } = new();
        public List<TrackIncome> TrackIncomes { get; set; } = new();
        public List<BudgetGoal> BudgetGoals { get; set; } = new();
        public List<ExpenseCategory> ExpenseCategories { get; set; } = new();
        public List<BudgetAlerts> BudgetAlerts { get; set; } = new();
        public List<DuesManagement> Dues { get; set; } = new();
        public List<RecurringTransaction> RecurringTransactions { get; set; } = new();
        public List<AuditLog> AuditLogs { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}