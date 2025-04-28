using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class RecurringTransaction
    {
        [Key]
        public string RecurringTransactionId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; } = decimal.Zero;

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public string? ExpenseCategoryId { get; set; }
        public ExpenseCategory? Category { get; set; }
    }
}