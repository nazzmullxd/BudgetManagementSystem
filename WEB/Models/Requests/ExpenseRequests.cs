using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Requests
{
    /// <summary>
    /// Request model for creating a new expense
    /// </summary>
    public class CreateExpenseRequest
    {
        /// <summary>
        /// Name of the expense item
        /// </summary>
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Item name must be between 1 and 50 characters")]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Price per unit of the item
        /// </summary>
        [Required(ErrorMessage = "Item price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Item price must be greater than 0")]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Quantity of items purchased
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; } = 1.0M;

        /// <summary>
        /// Date when the transaction occurred
        /// </summary>
        [Required(ErrorMessage = "Transaction date is required")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        /// <summary>
        /// ID of the expense category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string CategoryId { get; set; } = string.Empty;

        /// <summary>
        /// ID of the currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required")]
        public string CurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Optional tags to associate with this expense
        /// </summary>
        public List<string> TagIds { get; set; } = new();
    }

    /// <summary>
    /// Request model for updating an existing expense
    /// </summary>
    public class UpdateExpenseRequest
    {
        /// <summary>
        /// Name of the expense item
        /// </summary>
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Item name must be between 1 and 50 characters")]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Price per unit of the item
        /// </summary>
        [Required(ErrorMessage = "Item price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Item price must be greater than 0")]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Quantity of items purchased
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Date when the transaction occurred
        /// </summary>
        [Required(ErrorMessage = "Transaction date is required")]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// ID of the expense category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string CategoryId { get; set; } = string.Empty;

        /// <summary>
        /// ID of the currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required")]
        public string CurrencyId { get; set; } = string.Empty;

        /// <summary>
        /// Optional tags to associate with this expense
        /// </summary>
        public List<string> TagIds { get; set; } = new();
    }
}