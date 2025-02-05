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

        // If ExpenseCategory should be independent of TrackExpense,
        // consider removing this foreign key property or change your design.
        [Required]
        public string TrackExpenseId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
