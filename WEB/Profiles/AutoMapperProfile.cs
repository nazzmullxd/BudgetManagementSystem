using AutoMapper;
using Database.Model;
using WEB.Models.DTOs;
using WEB.Models.Requests;

namespace WEB.Profiles
{
    /// <summary>
    /// AutoMapper profile for configuring entity to DTO mappings
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigureExpenseMappings();
            ConfigureCategoryMappings();
            ConfigureIncomeMappings();
            ConfigureUserMappings();
            ConfigureCurrencyMappings();
            ConfigureTagMappings();
        }

        private void ConfigureExpenseMappings()
        {
            // TrackExpense to ExpenseDto
            CreateMap<TrackExpense, ExpenseDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TransactionTags.Select(tt => tt.Tag)));

            // CreateExpenseRequest to TrackExpense
            CreateMap<CreateExpenseRequest, TrackExpense>()
                .ForMember(dest => dest.TrackExpenseId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ExpenseCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionTags, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateExpenseRequest to TrackExpense
            CreateMap<UpdateExpenseRequest, TrackExpense>()
                .ForMember(dest => dest.TrackExpenseId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ExpenseCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionTags, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }

        private void ConfigureCategoryMappings()
        {
            // ExpenseCategory to CategoryDto
            CreateMap<ExpenseCategory, CategoryDto>()
                .ForMember(dest => dest.ExpenseCount, opt => opt.MapFrom(src => src.Expenses.Count))
                .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.Expenses.Sum(e => e.ItemPrice * e.Quantity)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // ExpenseCategory doesn't have CreatedAt

            // CategorySummaryDto mapping
            CreateMap<ExpenseCategory, CategorySummaryDto>();

            // CreateCategoryRequest to ExpenseCategory
            CreateMap<CreateCategoryRequest, ExpenseCategory>()
                .ForMember(dest => dest.ExpenseCategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Expenses, opt => opt.Ignore())
                .ForMember(dest => dest.RecurringTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetGoals, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Description));

            // UpdateCategoryRequest to ExpenseCategory
            CreateMap<UpdateCategoryRequest, ExpenseCategory>()
                .ForMember(dest => dest.ExpenseCategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Expenses, opt => opt.Ignore())
                .ForMember(dest => dest.RecurringTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.BudgetGoals, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Description));
        }

        private void ConfigureIncomeMappings()
        {
            // TrackIncome to IncomeDto
            CreateMap<TrackIncome, IncomeDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TransactionTags.Select(tt => tt.Tag)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // TrackIncome doesn't have CreatedAt
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // TrackIncome doesn't have UpdatedAt

            // CreateIncomeRequest to TrackIncome
            CreateMap<CreateIncomeRequest, TrackIncome>()
                .ForMember(dest => dest.IncomeId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionTags, opt => opt.Ignore())
                .ForMember(dest => dest.IncomeSource, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.IncomeAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.IncomeDescription, opt => opt.MapFrom(src => src.Description));

            // UpdateIncomeRequest to TrackIncome
            CreateMap<UpdateIncomeRequest, TrackIncome>()
                .ForMember(dest => dest.IncomeId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionTags, opt => opt.Ignore())
                .ForMember(dest => dest.IncomeSource, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.IncomeAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.IncomeDescription, opt => opt.MapFrom(src => src.Description));
        }

        private void ConfigureUserMappings()
        {
            // User to UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.TotalExpenses, opt => opt.MapFrom(src => src.TrackExpenses.Sum(e => e.ItemPrice * e.Quantity)))
                .ForMember(dest => dest.TotalIncome, opt => opt.MapFrom(src => src.TrackIncomes.Sum(i => i.IncomeAmount)))
                .ForMember(dest => dest.ExpenseCount, opt => opt.MapFrom(src => src.TrackExpenses.Count))
                .ForMember(dest => dest.IncomeCount, opt => opt.MapFrom(src => src.TrackIncomes.Count))
                .ForMember(dest => dest.CategoryCount, opt => opt.MapFrom(src => src.ExpenseCategories.Count));
        }

        private void ConfigureCurrencyMappings()
        {
            // Currency to CurrencySummaryDto
            CreateMap<Currency, CurrencySummaryDto>();
        }

        private void ConfigureTagMappings()
        {
            // Tag to TagSummaryDto
            CreateMap<Tag, TagSummaryDto>()
                .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => src.TransactionTags.Count));
        }
    }
}