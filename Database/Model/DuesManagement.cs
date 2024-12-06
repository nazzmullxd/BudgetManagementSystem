using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class DuesManagement
    {
        [Key]
        public string DueId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Payee { get; set; } = string.Empty; // Name of the person/entity to whom the due is owed

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Daily Limit Must be Positive")] 
        public decimal TotalDueAmount { get; set; } = decimal.Zero; // Total due amount

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Daily Limit Must be Positive")]
        public decimal PaidAmount { get; set; } = decimal.Zero; // Amount already paid

        // Calculated property for remaining due amount
        public decimal RemainingDue => TotalDueAmount - PaidAmount;

        [Required]
        public bool IsPaid { get; set; } = false; // Indicates whether the due is fully paid
    }
}
