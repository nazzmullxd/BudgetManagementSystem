using FluentValidation;
using WEB.Models.Requests;

namespace WEB.Validators
{
    /// <summary>
    /// Validator for CreateCategoryRequest
    /// </summary>
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters")
                .Must(NotContainSpecialCharacters).WithMessage("Category name can only contain letters, numbers, spaces, and hyphens");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Category description must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Color)
                .MaximumLength(7).WithMessage("Color code must not exceed 7 characters")
                .Must(BeValidHexColor).WithMessage("Color code must be a valid hex color (e.g., #FF5733)")
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.BudgetLimit)
                .GreaterThan(0).WithMessage("Budget limit must be greater than zero")
                .LessThan(1000000).WithMessage("Budget limit seems unrealistic (maximum $999,999)")
                .PrecisionScale(10, 2, false).WithMessage("Budget limit can have at most 2 decimal places")
                .When(x => x.BudgetLimit.HasValue);

            RuleFor(x => x.Icon)
                .MaximumLength(30).WithMessage("Icon name must not exceed 30 characters")
                .When(x => !string.IsNullOrEmpty(x.Icon));
        }

        private bool NotContainSpecialCharacters(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            
            // Allow letters, numbers, spaces, and hyphens only
            return name.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-');
        }

        private bool BeValidHexColor(string colorCode)
        {
            if (string.IsNullOrEmpty(colorCode)) return true; // Optional field
            
            // Must start with # and be followed by exactly 6 hex characters
            if (colorCode.Length != 7 || !colorCode.StartsWith("#"))
                return false;

            var hexPart = colorCode.Substring(1);
            return hexPart.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
        }
    }

    /// <summary>
    /// Validator for UpdateCategoryRequest
    /// </summary>
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name must not exceed 50 characters")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters")
                .Must(NotContainSpecialCharacters).WithMessage("Category name can only contain letters, numbers, spaces, and hyphens");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Category description must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Color)
                .MaximumLength(7).WithMessage("Color code must not exceed 7 characters")
                .Must(BeValidHexColor).WithMessage("Color code must be a valid hex color (e.g., #FF5733)")
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.BudgetLimit)
                .GreaterThan(0).WithMessage("Budget limit must be greater than zero")
                .LessThan(1000000).WithMessage("Budget limit seems unrealistic (maximum $999,999)")
                .PrecisionScale(10, 2, false).WithMessage("Budget limit can have at most 2 decimal places")
                .When(x => x.BudgetLimit.HasValue);

            RuleFor(x => x.Icon)
                .MaximumLength(30).WithMessage("Icon name must not exceed 30 characters")
                .When(x => !string.IsNullOrEmpty(x.Icon));
        }

        private bool NotContainSpecialCharacters(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            
            // Allow letters, numbers, spaces, and hyphens only
            return name.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-');
        }

        private bool BeValidHexColor(string colorCode)
        {
            if (string.IsNullOrEmpty(colorCode)) return true; // Optional field
            
            // Must start with # and be followed by exactly 6 hex characters
            if (colorCode.Length != 7 || !colorCode.StartsWith("#"))
                return false;

            var hexPart = colorCode.Substring(1);
            return hexPart.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
        }
    }
}