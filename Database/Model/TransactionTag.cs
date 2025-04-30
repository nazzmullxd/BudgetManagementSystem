using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class TransactionTag
    {
        [Key]
        public string TransactionTagId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string TagId { get; set; } = Guid.NewGuid().ToString();

        public Tag? Tag { get; set; }

        public string? TrackExpenseId { get; set; }
        public TrackExpense? Expense { get; set; }

        public string? TrackIncomeId { get; set; }
        public TrackIncome? Income { get; set; }
    }
}