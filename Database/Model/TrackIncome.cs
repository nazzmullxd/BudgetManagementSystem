using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class TrackIncome
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

        // Removed [MaxLength] because it does not apply to decimals.
        [Required]
        public decimal IncomeTax { get; set; } = decimal.Zero;

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
