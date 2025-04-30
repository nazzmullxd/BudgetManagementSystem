using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public partial class TrackIncome
    {
        [Key]
        public string IncomeId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string IncomeSource { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IncomeType { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IncomeDescription { get; set; } = string.Empty;

        [Required]
        public decimal IncomeAmount { get; set; } = decimal.Zero;

        [Required]
        public DateTime IncomeDate { get; set; }

        [Required]
        public decimal IncomeTax { get; set; } = decimal.Zero;

        public decimal NetIncome => IncomeAmount - IncomeTax;

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = "OneTime";

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Nullable navigation property
        public User? User { get; set; }

        [Required]
        public string CurrencyId { get; set; } = string.Empty;

        // Nullable navigation property
        public Currency? Currency { get; set; }

        public List<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
    }
}
