namespace WEB.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Category information
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Unique identifier for the category
        /// </summary>
        public string ExpenseCategoryId { get; set; } = string.Empty;

        /// <summary>
        /// Name of the category
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the category
        /// </summary>
        public string CategoryDescription { get; set; } = string.Empty;

        /// <summary>
        /// Number of expenses in this category
        /// </summary>
        public int ExpenseCount { get; set; }

        /// <summary>
        /// Total amount spent in this category
        /// </summary>
        public decimal TotalSpent { get; set; }

        /// <summary>
        /// When the category was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}