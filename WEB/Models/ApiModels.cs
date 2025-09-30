using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class CreateExpenseRequest
    {
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal ItemPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public string CurrencyId { get; set; } = string.Empty;
    }

    public class UpdateExpenseRequest
    {
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal ItemPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        [Required]
        public DateTime TransactionDate { get; set; }
    }

    public class CreateIncomeRequest
    {
        [Required]
        [MaxLength(50)]
        public string IncomeSource { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IncomeType { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IncomeDescription { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Income amount must be greater than 0")]
        public decimal IncomeAmount { get; set; }

        [Required]
        public DateTime IncomeDate { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue, ErrorMessage = "Tax amount cannot be negative")]
        public decimal IncomeTax { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = "OneTime";

        [Required]
        public string CurrencyId { get; set; } = string.Empty;
    }

    public class UpdateIncomeRequest
    {
        [Required]
        [MaxLength(50)]
        public string IncomeSource { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IncomeType { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IncomeDescription { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Income amount must be greater than 0")]
        public decimal IncomeAmount { get; set; }

        [Required]
        public DateTime IncomeDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax amount cannot be negative")]
        public decimal IncomeTax { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = "OneTime";
    }

    public class CreateBudgetGoalRequest
    {
        [Required]
        [MaxLength(100)]
        public string GoalName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class CreateCategoryRequest
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CategoryDescription { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? PreferredCurrencyId { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}