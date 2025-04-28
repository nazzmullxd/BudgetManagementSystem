using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class ExpenseCategory
    {
        [Key]
        public string ExpenseCategoryId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CategoryDescription { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public List<TrackExpense> Expenses { get; set; } = new List<TrackExpense>();
        public List<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
        public List<BudgetGoal> BudgetGoals { get; set; } = new List<BudgetGoal>();
    }
}