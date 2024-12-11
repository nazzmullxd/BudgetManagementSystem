using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class TrackIncome
    {
        [Key]
        public string IncomeId { get; set; }= Guid.NewGuid().ToString();
        [Required,MaxLength(50)]
        public string ?IncomeSource {  get; set; }
        [Required, MaxLength(50)]
        public string? IncomeType { get; set; }
        [Required, MaxLength(100)]
        public string? IncomeDescription { get; set; }
        [Required, MaxLength(50)]
        public decimal? IncomeAmount { get; set; }= decimal.Zero;
        [Required]
        public DateTime? IncomeDate { get; set; }
        [Required, MaxLength(50)]
        public decimal? IncomeTax {  get; set; }= decimal.Zero;
        [Required]
        public string ?User { get; set; }


    }
}
