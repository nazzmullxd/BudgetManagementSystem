using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class ExpenseCategory
    {
        [Key]
        public string ExpenseCategoryId { get; set; }= Guid.NewGuid().ToString();
        [Required]
        [MaxLength(50)]
        public string ? CategoryName { get; set; }
        [Required]
        [MaxLength(100)]
        public string? CategoryDescription { get; set; }
        [Required]
        public string? TrackExpenseId { get; set; }
        [Required]
        public string ?UserId { get; set; }

        }
}
