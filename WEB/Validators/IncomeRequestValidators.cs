using FluentValidation;
using WEB.Models.Requests;

namespace WEB.Validators
{
    /// <summary>
    /// Validator for CreateIncomeRequest
    /// </summary>
    public class CreateIncomeRequestValidator : AbstractValidator<CreateIncomeRequest>
    {
        private readonly string[] _validFrequencies = { "OneTime", "Daily", "Weekly", "BiWeekly", "Monthly", "Quarterly", "Annually" };
        private readonly string[] _validIncomeTypes = { "Salary", "Freelance", "Business", "Investment", "Rental", "Bonus", "Gift", "Other" };

        public CreateIncomeRequestValidator()
        {
            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Income source is required")
                .MaximumLength(50).WithMessage("Income source must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Income source must be at least 2 characters");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Income amount must be greater than zero")
                .LessThan(10000000).WithMessage("Income amount seems unrealistic (maximum $9,999,999)")
                .PrecisionScale(10, 2, false).WithMessage("Income amount can have at most 2 decimal places");

            RuleFor(x => x.IncomeDate)
                .NotEmpty().WithMessage("Income date is required")
                .LessThanOrEqualTo(DateTime.Today.AddDays(30)).WithMessage("Income date cannot be more than 30 days in the future")
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Income date cannot be before January 1, 1900");

            RuleFor(x => x.IncomeType)
                .NotEmpty().WithMessage("Income type is required")
                .MaximumLength(50).WithMessage("Income type must not exceed 50 characters")
                .Must(BeValidIncomeType).WithMessage($"Income type must be one of: {string.Join(", ", _validIncomeTypes)}");

            RuleFor(x => x.IncomeTax)
                .GreaterThanOrEqualTo(0).WithMessage("Income tax cannot be negative")
                .LessThan(x => x.Amount).WithMessage("Income tax cannot be greater than or equal to income amount")
                .PrecisionScale(10, 2, false).WithMessage("Income tax can have at most 2 decimal places");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required")
                .MaximumLength(50).WithMessage("Frequency must not exceed 50 characters")
                .Must(BeValidFrequency).WithMessage($"Frequency must be one of: {string.Join(", ", _validFrequencies)}");

            RuleFor(x => x.CurrencyId)
                .NotEmpty().WithMessage("Currency is required")
                .Length(36).WithMessage("Currency ID must be a valid GUID");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            // Business logic validation
            RuleFor(x => x)
                .Must(HaveReasonableNetIncome)
                .WithMessage("Net income (amount - tax) cannot be negative or zero")
                .WithName("NetIncome");

            RuleFor(x => x)
                .Must(HaveReasonableTaxRate)
                .WithMessage("Tax rate seems unrealistic (maximum 80%)")
                .WithName("TaxRate");
        }

        private bool BeValidFrequency(string frequency)
        {
            return _validFrequencies.Contains(frequency, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidIncomeType(string incomeType)
        {
            return _validIncomeTypes.Contains(incomeType, StringComparer.OrdinalIgnoreCase);
        }

        private bool HaveReasonableNetIncome(CreateIncomeRequest request)
        {
            var netIncome = request.Amount - request.IncomeTax;
            return netIncome > 0;
        }

        private bool HaveReasonableTaxRate(CreateIncomeRequest request)
        {
            var taxRate = (request.IncomeTax / request.Amount) * 100;
            return taxRate <= 80; // Maximum 80% tax rate
        }
    }

    /// <summary>
    /// Validator for UpdateIncomeRequest
    /// </summary>
    public class UpdateIncomeRequestValidator : AbstractValidator<UpdateIncomeRequest>
    {
        private readonly string[] _validFrequencies = { "OneTime", "Daily", "Weekly", "BiWeekly", "Monthly", "Quarterly", "Annually" };
        private readonly string[] _validIncomeTypes = { "Salary", "Freelance", "Business", "Investment", "Rental", "Bonus", "Gift", "Other" };

        public UpdateIncomeRequestValidator()
        {
            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Income source is required")
                .MaximumLength(50).WithMessage("Income source must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Income source must be at least 2 characters");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Income amount must be greater than zero")
                .LessThan(10000000).WithMessage("Income amount seems unrealistic (maximum $9,999,999)")
                .PrecisionScale(10, 2, false).WithMessage("Income amount can have at most 2 decimal places");

            RuleFor(x => x.IncomeDate)
                .NotEmpty().WithMessage("Income date is required")
                .LessThanOrEqualTo(DateTime.Today.AddDays(30)).WithMessage("Income date cannot be more than 30 days in the future")
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Income date cannot be before January 1, 1900");

            RuleFor(x => x.IncomeType)
                .NotEmpty().WithMessage("Income type is required")
                .MaximumLength(50).WithMessage("Income type must not exceed 50 characters")
                .Must(BeValidIncomeType).WithMessage($"Income type must be one of: {string.Join(", ", _validIncomeTypes)}");

            RuleFor(x => x.IncomeTax)
                .GreaterThanOrEqualTo(0).WithMessage("Income tax cannot be negative")
                .LessThan(x => x.Amount).WithMessage("Income tax cannot be greater than or equal to income amount")
                .PrecisionScale(10, 2, false).WithMessage("Income tax can have at most 2 decimal places");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required")
                .MaximumLength(50).WithMessage("Frequency must not exceed 50 characters")
                .Must(BeValidFrequency).WithMessage($"Frequency must be one of: {string.Join(", ", _validFrequencies)}");

            RuleFor(x => x.CurrencyId)
                .NotEmpty().WithMessage("Currency is required")
                .Length(36).WithMessage("Currency ID must be a valid GUID");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            // Business logic validation
            RuleFor(x => x)
                .Must(HaveReasonableNetIncome)
                .WithMessage("Net income (amount - tax) cannot be negative or zero")
                .WithName("NetIncome");

            RuleFor(x => x)
                .Must(HaveReasonableTaxRate)
                .WithMessage("Tax rate seems unrealistic (maximum 80%)")
                .WithName("TaxRate");
        }

        private bool BeValidFrequency(string frequency)
        {
            return _validFrequencies.Contains(frequency, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidIncomeType(string incomeType)
        {
            return _validIncomeTypes.Contains(incomeType, StringComparer.OrdinalIgnoreCase);
        }

        private bool HaveReasonableNetIncome(UpdateIncomeRequest request)
        {
            var netIncome = request.Amount - request.IncomeTax;
            return netIncome > 0;
        }

        private bool HaveReasonableTaxRate(UpdateIncomeRequest request)
        {
            var taxRate = (request.IncomeTax / request.Amount) * 100;
            return taxRate <= 80; // Maximum 80% tax rate
        }
    }
}