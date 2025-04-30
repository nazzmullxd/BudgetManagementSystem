using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Database.Model
{
    public class RecurringTransaction
    {
        [Key]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        public User? User { get; set; }

        [Required]
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();

        public ExpenseCategory? Category { get; set; }

        [Required]
        public string CurrencyId { get; set; } = Guid.NewGuid().ToString();

        public Currency? Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Frequency { get; set; } = "Monthly";

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
    }
}