using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Legacy property name for compatibility
        [NotMapped]
        public string ExpenseCategoryId
        {
            get => CategoryId;
            set => CategoryId = value;
        }

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

        public ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
    }
}