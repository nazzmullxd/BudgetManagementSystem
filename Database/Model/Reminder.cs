using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    //Reminder for Over Expense
    public class Reminder
    {
        [Key]
        public string ReminderId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public Decimal ExpenseLimit { get; set; }
        
        








    }
}
