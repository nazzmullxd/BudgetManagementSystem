using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Model
{
    public class TrackExpense
    {
        [Key]
        public string TrackExpenseId { get; set; } = Guid.NewGuid().ToString();
        [Required, MaxLength(50)]
        public string? ItemName { get; set; }
        [Required]
        public decimal? ItemPrice { get; set; }
        [Required]
        public decimal? ItemAmount { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime? UpdatedAt { get;set; }
        [Required]
        public string ?User { get; set; }
        public string ? ExpenseCategoryId { get; set;}



    }
}
