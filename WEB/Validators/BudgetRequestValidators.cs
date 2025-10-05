using FluentValidation;
using WEB.Models.Requests;

namespace WEB.Validators
{
    public class CreateBudgetGoalRequestValidator : AbstractValidator<CreateBudgetGoalRequest>
    {
        public CreateBudgetGoalRequestValidator()
        {
            RuleFor(x => x.GoalName)
                .NotEmpty().WithMessage("Goal name is required")
                .Length(1, 100).WithMessage("Goal name must be between 1 and 100 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Goal name can only contain letters, numbers, spaces, hyphens, and underscores");

            RuleFor(x => x.TargetAmount)
                .NotEmpty().WithMessage("Target amount is required")
                .GreaterThan(0).WithMessage("Target amount must be greater than 0")
                .LessThanOrEqualTo(999999999.99m).WithMessage("Target amount cannot exceed 999,999,999.99")
                .PrecisionScale(12, 2, false).WithMessage("Target amount must have at most 2 decimal places");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(BeValidDate).WithMessage("Start date must be a valid date")
                .Must(BeCurrentOrFutureDate).WithMessage("Start date cannot be in the past");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .Must(BeValidDate).WithMessage("End date must be a valid date");

            RuleFor(x => x.ExpenseCategoryId)
                .NotEmpty().WithMessage("Expense category is required")
                .Must(BeValidGuid).WithMessage("Invalid expense category ID format");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            // Business logic validation
            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("End date must be after start date")
                .When(x => x.StartDate != default && x.EndDate != default);

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays <= 3650) // Max 10 years
                .WithMessage("Budget goal duration cannot exceed 10 years")
                .When(x => x.StartDate != default && x.EndDate != default && x.EndDate > x.StartDate);

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays >= 1) // Minimum 1 day
                .WithMessage("Budget goal must be at least 1 day long")
                .When(x => x.StartDate != default && x.EndDate != default && x.EndDate > x.StartDate);
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default(DateTime) && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }

        private bool BeCurrentOrFutureDate(DateTime date)
        {
            return date.Date >= DateTime.Today;
        }

        private bool BeValidGuid(string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }
    }

    public class UpdateBudgetGoalRequestValidator : AbstractValidator<UpdateBudgetGoalRequest>
    {
        public UpdateBudgetGoalRequestValidator()
        {
            RuleFor(x => x.GoalName)
                .NotEmpty().WithMessage("Goal name is required")
                .Length(1, 100).WithMessage("Goal name must be between 1 and 100 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Goal name can only contain letters, numbers, spaces, hyphens, and underscores");

            RuleFor(x => x.TargetAmount)
                .NotEmpty().WithMessage("Target amount is required")
                .GreaterThan(0).WithMessage("Target amount must be greater than 0")
                .LessThanOrEqualTo(999999999.99m).WithMessage("Target amount cannot exceed 999,999,999.99")
                .PrecisionScale(12, 2, false).WithMessage("Target amount must have at most 2 decimal places");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(BeValidDate).WithMessage("Start date must be a valid date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .Must(BeValidDate).WithMessage("End date must be a valid date");

            RuleFor(x => x.ExpenseCategoryId)
                .NotEmpty().WithMessage("Expense category is required")
                .Must(BeValidGuid).WithMessage("Invalid expense category ID format");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            // Business logic validation
            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("End date must be after start date")
                .When(x => x.StartDate != default && x.EndDate != default);

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays <= 3650) // Max 10 years
                .WithMessage("Budget goal duration cannot exceed 10 years")
                .When(x => x.StartDate != default && x.EndDate != default && x.EndDate > x.StartDate);

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays >= 1) // Minimum 1 day
                .WithMessage("Budget goal must be at least 1 day long")
                .When(x => x.StartDate != default && x.EndDate != default && x.EndDate > x.StartDate);
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default(DateTime) && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }

        private bool BeValidGuid(string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }
    }
}