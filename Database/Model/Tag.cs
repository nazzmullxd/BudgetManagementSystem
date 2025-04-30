using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Tag
    {
        [Key]
        public string TagId { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string TagName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public User? User { get; set; }

        public List<TransactionTag> TransactionTags { get; set; } = new();
    }
}
