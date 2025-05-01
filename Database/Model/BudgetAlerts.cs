using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class BudgetAlerts
    {
        [Key]
        public string BudgetAlertsId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Daily limit must be positive.")]
        public decimal DailyLimit { get; set; } = decimal.Zero;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weekly limit must be positive.")]
        public decimal WeeklyLimit { get; set; } = decimal.Zero;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly limit must be positive.")]
        public decimal MonthlyLimit { get; set; } = decimal.Zero;

        [Required]
        public decimal ThresholdPercentage { get; set; } = 0.9M;

        [Required]
        public bool IsSent { get; set; } = false;

        public DateTime? LastTriggered { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}