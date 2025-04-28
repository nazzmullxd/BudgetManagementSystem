using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public partial class User
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        public string? PreferredCurrencyId { get; set; }
        public Currency? PreferredCurrency { get; set; }

        public List<TrackExpense> Expenses { get; set; } = new List<TrackExpense>();
        public List<TrackIncome> Incomes { get; set; } = new List<TrackIncome>();
        public List<ExpenseCategory> Categories { get; set; } = new List<ExpenseCategory>();
        public List<DuesManagement> Dues { get; set; } = new List<DuesManagement>();
        public List<BudgetAlerts> BudgetAlerts { get; set; } = new List<BudgetAlerts>();
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();
        public List<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public List<BudgetGoal> BudgetGoals { get; set; } = new List<BudgetGoal>();
    }
}