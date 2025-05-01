using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class AuditLog
    {
        [Key]
        public string AuditLogId { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Required, MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public string EntityId { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Details { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Nullable navigation property
        public User? User { get; set; }
    }
}
