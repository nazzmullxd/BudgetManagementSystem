using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public partial class TrackExpense
    {
        [Key]
        public string TrackExpenseId { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public decimal ItemPrice { get; set; } = decimal.Zero;

        [Required]
        public decimal Quantity { get; set; } = 1.0M;

        public decimal TotalCost => ItemPrice * Quantity;

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public User? User { get; set; }

        [Required]
        public string ExpenseCategoryId { get; set; } = string.Empty;

        public ExpenseCategory? Category { get; set; }

        [Required]
        public string CurrencyId { get; set; } = string.Empty;

        public Currency? Currency { get; set; }

        public List<TransactionTag> TransactionTags { get; set; } = new();
    }
}
