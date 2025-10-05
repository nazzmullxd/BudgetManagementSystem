namespace WEB.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Income information
    /// </summary>
    public class IncomeDto
    {
        /// <summary>
        /// Unique identifier for the income
        /// </summary>
        public string IncomeId { get; set; } = string.Empty;

        /// <summary>
        /// Source of the income
        /// </summary>
        public string IncomeSource { get; set; } = string.Empty;

        /// <summary>
        /// Amount of income
        /// </summary>
        public decimal IncomeAmount { get; set; }

        /// <summary>
        /// Date when the income was received
        /// </summary>
        public DateTime IncomeDate { get; set; }

        /// <summary>
        /// Income type
        /// </summary>
        public string IncomeType { get; set; } = string.Empty;

        /// <summary>
        /// Tax amount on this income
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Net income after tax
        /// </summary>
        public decimal NetIncome { get; set; }

        /// <summary>
        /// Frequency of this income
        /// </summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Currency information
        /// </summary>
        public CurrencySummaryDto? Currency { get; set; }

        /// <summary>
        /// Description or notes about the income
        /// </summary>
        public string IncomeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Tags associated with this income
        /// </summary>
        public List<TagSummaryDto> Tags { get; set; } = new();

        /// <summary>
        /// When the income record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the income record was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}