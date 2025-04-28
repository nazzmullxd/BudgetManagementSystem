using Database.Model;
using Database.Context;
using BudgetManagementSystem.Repositories;  // This is correct
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BudgetManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection
            var services = new ServiceCollection();

            // Register DbContext
            services.AddDbContext<BudgetManagementContext>(options =>
                options.UseSqlServer(@"Server=NASIM\MSSQLSERVER01;Database=BudgetManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;"));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IDuesManagementRepository, DuesManagementRepository>();
            services.AddScoped<IBudgetAlertsRepository, BudgetAlertsRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITransactionTagRepository, TransactionTagRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IBudgetGoalRepository, BudgetGoalRepository>();

            var serviceProvider = services.BuildServiceProvider();

            // Resolve repositories
            using var scope = serviceProvider.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var expenseRepo = scope.ServiceProvider.GetRequiredService<IExpenseRepository>();
            var categoryRepo = scope.ServiceProvider.GetRequiredService<IExpenseCategoryRepository>();
            var currencyRepo = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();
            var reminderRepo = scope.ServiceProvider.GetRequiredService<IReminderRepository>();

            // Ensure the database is created
            var context = scope.ServiceProvider.GetRequiredService<BudgetManagementContext>();
            context.Database.EnsureCreated();

            // Test the repositories
            var user = new User
            {
                Name = "Test User",
                Email = "test5@example.com",
                PasswordHash = "hashedpassword",
            };
            userRepo.Add(user);

            var category = new ExpenseCategory
            {
                CategoryName = "Food",
                CategoryDescription = "Food expenses",
                UserId = user.UserId
            };
            categoryRepo.Add(category);

            var currency = new Currency
            {
                Code = "USD",
                Name = "US Dollar",
                ExchangeRateToBase = 1.0M
            };
            currencyRepo.Add(currency);

            var expense = new TrackExpense
            {
                ItemName = "Groceries",
                ItemPrice = 50.0M,
                Quantity = 1,
                UserId = user.UserId,
                ExpenseCategoryId = category.ExpenseCategoryId,
                CurrencyId = currency.CurrencyId
            };
            expenseRepo.Add(expense);

            var reminder = new Reminder
            {
                Description = "Pay rent",
                DueDate = DateTime.Now.AddDays(1),
                UserId = user.UserId
            };
            reminderRepo.Add(reminder);

            var expenses = expenseRepo.GetByUserId(user.UserId);
            foreach (var exp in expenses)
            {
                Console.WriteLine($"Expense: {exp.ItemName}, Total Cost: {exp.TotalCost}, Category: {exp.Category.CategoryName}");
            }

            var reminders = reminderRepo.GetUpcomingReminders(user.UserId, DateTime.Now.AddDays(2));
            foreach (var rem in reminders)
            {
                Console.WriteLine($"Reminder: {rem.Description}, Due: {rem.DueDate}");
            }
        }
    }
}