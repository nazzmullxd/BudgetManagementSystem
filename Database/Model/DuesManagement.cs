using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class DuesManagement
    {
        [Key]
        public string DueId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string Payee { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total due amount must be positive.")]
        public decimal TotalDueAmount { get; set; } = decimal.Zero;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be positive.")]
        public decimal PaidAmount { get; set; } = decimal.Zero;

        public decimal RemainingDue => TotalDueAmount - PaidAmount;

        [Required]
        public bool IsPaid { get; set; } = false;

        public DateTime? ReminderDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}