using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Reminder
    {
        [Key]
        public string ReminderId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public bool IsSent { get; set; } = false;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}