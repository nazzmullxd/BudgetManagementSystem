using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Requests
{
    /// <summary>
    /// Request model for creating a new income entry
    /// </summary>
    public class CreateIncomeRequest
    {
        /// <summary>
        /// Source of the income (e.g., Salary, Freelance, Investment)
        /// </summary>
        [Required(ErrorMessage = "Income source is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Income source must be between 1 and 50 characters")]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Amount of income received
        /// </summary>
        [Required(ErrorMessage = "Income amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Income amount must be greater than 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date when the income was received
        /// </summary>
        [Required(ErrorMessage = "Income date is required")]
        public DateTime IncomeDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Type of income (e.g., Salary, Bonus, Investment)
        /// </summary>
        [Required(ErrorMessage = "Income type is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Income type must be between 1 and 50 characters")]
        public string IncomeType { get; set; } = string.Empty;

        /// <summary>
        /// Tax amount on this income
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Tax amount cannot be negative")]
        public decimal IncomeTax { get; set; } = 0;

        /// <summary>
        /// Frequency of this income (OneTime, Weekly, Monthly, etc.)
        /// </summary>
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        public string Frequency { get; set; } = "OneTime";

        /// <summary>
        /// Optional description or notes about the income
        /// </summary>
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// ID of the currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required")]
        public string CurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Whether this is a recurring income
        /// </summary>
        public bool IsRecurring { get; set; } = false;

        /// <summary>
        /// Type of recurring income (Monthly, Weekly, etc.) - only if IsRecurring is true
        /// </summary>
        [StringLength(20, ErrorMessage = "Recurrence type cannot exceed 20 characters")]
        public string? RecurrenceType { get; set; }

        /// <summary>
        /// Optional tags to associate with this income
        /// </summary>
        public List<string> TagIds { get; set; } = new();
    }

    /// <summary>
    /// Request model for updating an existing income entry
    /// </summary>
    public class UpdateIncomeRequest
    {
        /// <summary>
        /// Source of the income (e.g., Salary, Freelance, Investment)
        /// </summary>
        [Required(ErrorMessage = "Income source is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Income source must be between 1 and 50 characters")]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Amount of income received
        /// </summary>
        [Required(ErrorMessage = "Income amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Income amount must be greater than 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date when the income was received
        /// </summary>
        [Required(ErrorMessage = "Income date is required")]
        public DateTime IncomeDate { get; set; }

        /// <summary>
        /// Optional description or notes about the income
        /// </summary>
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// ID of the currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required")]
        public string CurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Whether this is a recurring income
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Type of recurring income (Monthly, Weekly, etc.) - only if IsRecurring is true
        /// </summary>
        [StringLength(20, ErrorMessage = "Recurrence type cannot exceed 20 characters")]
        public string? RecurrenceType { get; set; }

        /// <summary>
        /// Optional tags to associate with this income
        /// </summary>
        public List<string> TagIds { get; set; } = new();

        /// <summary>
        /// Type of income (e.g., Salary, Bonus, Investment)
        /// </summary>
        [Required(ErrorMessage = "Income type is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Income type must be between 1 and 50 characters")]
        public string IncomeType { get; set; } = string.Empty;

        /// <summary>
        /// Tax amount on this income
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Tax amount cannot be negative")]
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Frequency of this income (OneTime, Weekly, Monthly, etc.)
        /// </summary>
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        public string Frequency { get; set; } = "OneTime";
    }
}