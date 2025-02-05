using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Model
{
    public class TrackExpense
    {
        [Key]
        public string TrackExpenseId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public decimal ItemPrice { get; set; } = decimal.Zero;

        [Required]
        public decimal ItemAmount { get; set; } = decimal.Zero;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // If this is a foreign key, consider adding a navigation property.
        public string ExpenseCategoryId { get; set; } = string.Empty;
    }
}
