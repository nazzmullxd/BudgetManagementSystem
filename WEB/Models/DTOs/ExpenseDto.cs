using System.ComponentModel.DataAnnotations;

namespace WEB.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Expense information
    /// </summary>
    public class ExpenseDto
    {
        /// <summary>
        /// Unique identifier for the expense
        /// </summary>
        public string TrackExpenseId { get; set; } = string.Empty;

        /// <summary>
        /// Name of the expense item
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Price per unit of the item
        /// </summary>
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Quantity of items purchased
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Total cost (calculated: ItemPrice * Quantity)
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Date when the transaction occurred
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Expense category information
        /// </summary>
        public CategorySummaryDto? Category { get; set; }

        /// <summary>
        /// Currency information
        /// </summary>
        public CurrencySummaryDto? Currency { get; set; }

        /// <summary>
        /// Tags associated with this expense
        /// </summary>
        public List<TagSummaryDto> Tags { get; set; } = new();

        /// <summary>
        /// When the expense was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the expense was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Simplified category information for DTOs
    /// </summary>
    public class CategorySummaryDto
    {
        public string ExpenseCategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }

    /// <summary>
    /// Simplified currency information for DTOs
    /// </summary>
    public class CurrencySummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }

    /// <summary>
    /// Simplified tag information for DTOs
    /// </summary>
    public class TagSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int UsageCount { get; set; }
    }
}