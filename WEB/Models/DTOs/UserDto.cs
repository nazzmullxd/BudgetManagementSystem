namespace WEB.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for User information (safe for API exposure)
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// User's full name (computed)
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's preferred currency
        /// </summary>
        public CurrencySummaryDto? PreferredCurrency { get; set; }

        /// <summary>
        /// When the user account was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the user account was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Total amount spent by user
        /// </summary>
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// Total income received by user
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// Number of expense records
        /// </summary>
        public int ExpenseCount { get; set; }

        /// <summary>
        /// Number of income records
        /// </summary>
        public int IncomeCount { get; set; }

        /// <summary>
        /// Number of categories created
        /// </summary>
        public int CategoryCount { get; set; }
    }
}