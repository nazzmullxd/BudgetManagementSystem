using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class Currency
    {
        [Key]
        public string CurrencyId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public decimal ExchangeRateToBase { get; set; } = 1.0M;
    }
}