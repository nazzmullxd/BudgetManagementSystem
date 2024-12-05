using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class TrackIncome
    {
        [Key]
        public string IncomeId { get; set; }= Guid.NewGuid().ToString();
        [Required]
        public string ?IncomeSource {  get; set; }
        [Required]
        public string? IncomeType { get; set; }
        [Required]
        public string? IncomeDescription { get; set; }
        [Required]
        public decimal? IncomeAmount { get; set; }= decimal.Zero;
        [Required]
        public DateTime? IncomeDate { get; set; }
        [Required]
        public decimal? IncomeTax {  get; set; }= decimal.Zero;


    }
}
