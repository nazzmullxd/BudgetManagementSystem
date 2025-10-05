using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Requests
{
    /// <summary>
    /// Request model for creating a new budget goal
    /// </summary>
    public class CreateBudgetGoalRequest
    {
        /// <summary>
        /// Name of the budget goal
        /// </summary>
        [Required(ErrorMessage = "Goal name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Goal name must be between 1 and 100 characters")]
        public string GoalName { get; set; } = string.Empty;

        /// <summary>
        /// Target amount for the goal
        /// </summary>
        [Required(ErrorMessage = "Target amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// Start date of the budget goal
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// End date of the budget goal
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Associated expense category ID
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the goal
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for updating an existing budget goal
    /// </summary>
    public class UpdateBudgetGoalRequest
    {
        /// <summary>
        /// Name of the budget goal
        /// </summary>
        [Required(ErrorMessage = "Goal name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Goal name must be between 1 and 100 characters")]
        public string GoalName { get; set; } = string.Empty;

        /// <summary>
        /// Target amount for the goal
        /// </summary>
        [Required(ErrorMessage = "Target amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// Start date of the budget goal
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the budget goal
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Associated expense category ID
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the goal
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}