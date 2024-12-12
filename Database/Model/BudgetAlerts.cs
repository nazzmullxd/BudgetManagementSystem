using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class BudgetAlerts
    {
        [Key]
        public string BudgetAlertsId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Range (0,double.MaxValue,ErrorMessage ="Daily Limit Must be Positive")]
        public decimal DailyLimit { get; set; } = decimal.Zero;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Daily Limit Must be Positive")]
        public decimal WeeklyLimit { get; set; } = decimal.Zero;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Daily Limit Must be Positive")]
        public decimal MonthlyLimit { get; set; } = decimal.Zero;
        [Required]
        public decimal ThreesholdPercentage { get; set; } = 0.9M;
        [Required]
        public bool IsSent { get; set; } = false;
        public String ?UserId { get; set; }
        public String ? TrackExpenseID { get; set; }

    }
}
