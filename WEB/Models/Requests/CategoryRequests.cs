using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Requests
{
    /// <summary>
    /// Request model for creating a new category
    /// </summary>
    public class CreateCategoryRequest
    {
        /// <summary>
        /// Name of the category
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the category
        /// </summary>
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Color code for the category (e.g., #FF5733)
        /// </summary>
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color format. Use hex format like #FF5733")]
        public string? Color { get; set; }

        /// <summary>
        /// Icon name for the category
        /// </summary>
        [StringLength(30, ErrorMessage = "Icon name cannot exceed 30 characters")]
        public string? Icon { get; set; }

        /// <summary>
        /// Maximum budget limit for this category
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Budget limit must be greater than 0")]
        public decimal? BudgetLimit { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing category
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <summary>
        /// Name of the category
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the category
        /// </summary>
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Color code for the category (e.g., #FF5733)
        /// </summary>
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color format. Use hex format like #FF5733")]
        public string? Color { get; set; }

        /// <summary>
        /// Icon name for the category
        /// </summary>
        [StringLength(30, ErrorMessage = "Icon name cannot exceed 30 characters")]
        public string? Icon { get; set; }

        /// <summary>
        /// Maximum budget limit for this category
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Budget limit must be greater than 0")]
        public decimal? BudgetLimit { get; set; }
    }
}