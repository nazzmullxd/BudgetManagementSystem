using FluentValidation;
using WEB.Models.Requests;

namespace WEB.Validators
{
    public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
    {
        public CreateExpenseRequestValidator()
        {
            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item name is required")
                .Length(1, 50).WithMessage("Item name must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_\.\,\(\)]+$").WithMessage("Item name contains invalid characters");

            RuleFor(x => x.ItemPrice)
                .NotEmpty().WithMessage("Item price is required")
                .GreaterThan(0).WithMessage("Item price must be greater than 0")
                .LessThanOrEqualTo(999999999.99m).WithMessage("Item price cannot exceed 999,999,999.99")
                .PrecisionScale(12, 2, false).WithMessage("Item price must have at most 2 decimal places");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(99999).WithMessage("Quantity cannot exceed 99,999")
                .PrecisionScale(10, 3, false).WithMessage("Quantity must have at most 3 decimal places");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction date is required")
                .Must(BeValidDate).WithMessage("Transaction date must be a valid date")
                .Must(BeReasonableDate).WithMessage("Transaction date must be within the last 10 years and not in the future");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required")
                .Must(BeValidGuid).WithMessage("Invalid category ID format");

            RuleFor(x => x.CurrencyId)
                .NotEmpty().WithMessage("Currency is required")
                .Must(BeValidGuid).WithMessage("Invalid currency ID format");

            RuleForEach(x => x.TagIds)
                .Must(BeValidGuid).WithMessage("Invalid tag ID format")
                .When(x => x.TagIds != null && x.TagIds.Any());

            RuleFor(x => x.TagIds)
                .Must(x => x == null || x.Count <= 10).WithMessage("Maximum 10 tags allowed per expense");

            // Business logic validation - total cost validation
            RuleFor(x => x)
                .Must(x => x.ItemPrice * x.Quantity <= 999999999.99m)
                .WithMessage("Total cost (price × quantity) cannot exceed 999,999,999.99")
                .When(x => x.ItemPrice > 0 && x.Quantity > 0);
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default(DateTime) && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }

        private bool BeReasonableDate(DateTime date)
        {
            var tenYearsAgo = DateTime.Now.AddYears(-10);
            var today = DateTime.Now.Date.AddDays(1); // Allow until end of today
            return date >= tenYearsAgo && date < today;
        }

        private bool BeValidGuid(string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }
    }

    public class UpdateExpenseRequestValidator : AbstractValidator<UpdateExpenseRequest>
    {
        public UpdateExpenseRequestValidator()
        {
            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item name is required")
                .Length(1, 50).WithMessage("Item name must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_\.\,\(\)]+$").WithMessage("Item name contains invalid characters");

            RuleFor(x => x.ItemPrice)
                .NotEmpty().WithMessage("Item price is required")
                .GreaterThan(0).WithMessage("Item price must be greater than 0")
                .LessThanOrEqualTo(999999999.99m).WithMessage("Item price cannot exceed 999,999,999.99")
                .PrecisionScale(12, 2, false).WithMessage("Item price must have at most 2 decimal places");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(99999).WithMessage("Quantity cannot exceed 99,999")
                .PrecisionScale(10, 3, false).WithMessage("Quantity must have at most 3 decimal places");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction date is required")
                .Must(BeValidDate).WithMessage("Transaction date must be a valid date")
                .Must(BeReasonableDate).WithMessage("Transaction date must be within the last 10 years and not in the future");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required")
                .Must(BeValidGuid).WithMessage("Invalid category ID format");

            RuleFor(x => x.CurrencyId)
                .NotEmpty().WithMessage("Currency is required")
                .Must(BeValidGuid).WithMessage("Invalid currency ID format");

            RuleForEach(x => x.TagIds)
                .Must(BeValidGuid).WithMessage("Invalid tag ID format")
                .When(x => x.TagIds != null && x.TagIds.Any());

            RuleFor(x => x.TagIds)
                .Must(x => x == null || x.Count <= 10).WithMessage("Maximum 10 tags allowed per expense");

            // Business logic validation - total cost validation
            RuleFor(x => x)
                .Must(x => x.ItemPrice * x.Quantity <= 999999999.99m)
                .WithMessage("Total cost (price × quantity) cannot exceed 999,999,999.99")
                .When(x => x.ItemPrice > 0 && x.Quantity > 0);
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default(DateTime) && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }

        private bool BeReasonableDate(DateTime date)
        {
            var tenYearsAgo = DateTime.Now.AddYears(-10);
            var today = DateTime.Now.Date.AddDays(1); // Allow until end of today
            return date >= tenYearsAgo && date < today;
        }

        private bool BeValidGuid(string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }
    }
}