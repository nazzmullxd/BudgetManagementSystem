using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? PreferredCurrencyId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Computed property for full name
        [NotMapped]
        public string Name => $"{FirstName} {LastName}".Trim();

        // Navigation properties
        public Currency? PreferredCurrency { get; set; }
        
        // Collection navigation properties
        public ICollection<TrackExpense> Expenses { get; set; } = new List<TrackExpense>();
        public ICollection<TrackIncome> Incomes { get; set; } = new List<TrackIncome>();
        public ICollection<ExpenseCategory> Categories { get; set; } = new List<ExpenseCategory>();
        public ICollection<BudgetGoal> BudgetGoals { get; set; } = new List<BudgetGoal>();
        public ICollection<BudgetAlerts> BudgetAlerts { get; set; } = new List<BudgetAlerts>();
        public ICollection<DuesManagement> Dues { get; set; } = new List<DuesManagement>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
        public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        
        // Legacy properties for backward compatibility
        [NotMapped]
        public ICollection<TrackExpense> TrackExpenses => Expenses;
        [NotMapped]
        public ICollection<TrackIncome> TrackIncomes => Incomes;
        [NotMapped]
        public ICollection<ExpenseCategory> ExpenseCategories => Categories;
    }
}