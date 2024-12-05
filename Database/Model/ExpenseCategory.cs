using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class ExpenseCategory
    {
        [Key]
        public string CategoryId { get; set; }= Guid.NewGuid().ToString();
        [Required]
        [MaxLength(100)]
        public string ? CategoryName { get; set; }
        [Required]
        [MaxLength(100)]
        public string? CategoryDescription { get; set; }

        }
}
