using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class BudgetGoal
    {
        [Key]
        public string BudgetGoalId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(100)]
        public string GoalName { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Target amount must be positive.")]
        public decimal TargetAmount { get; set; } = decimal.Zero;

        [Required]
        public DateTime TargetDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Current amount must be positive.")]
        public decimal CurrentAmount { get; set; } = decimal.Zero;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public string? ExpenseCategoryId { get; set; }
        public ExpenseCategory? Category { get; set; }
    }
}