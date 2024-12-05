using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Model
{
    public class TrackExpense
    {
        [Key]
        public string ItemId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? ItemName { get; set; }
        [Required]
        public decimal? ItemPrice { get; set; }
        [Required]
        public decimal? ItemAmount { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime? UpdatedAt { get;set; }
        


    
    }
}
