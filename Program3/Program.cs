using Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection
            var serviceProvider = ConfigureServices();

            // Resolve services
            var userService = serviceProvider.GetService<IUserService>();
            var categoryService = serviceProvider.GetService<ICategoryService>();
            var currencyService = serviceProvider.GetService<ICurrencyService>();
            var expenseService = serviceProvider.GetService<IExpenseService>();
            var incomeService = serviceProvider.GetService<IIncomeService>();
            var tagService = serviceProvider.GetService<ITagService>();
            var budgetService = serviceProvider.GetService<IBudgetService>();
            var reportService = serviceProvider.GetService<IReportService>();
            var reminderService = serviceProvider.GetService<IReminderService>();
            var recurringTransactionService = serviceProvider.GetService<IRecurringTransactionService>();
            var alertService = serviceProvider.GetService<IAlertService>();
            var duesService = serviceProvider.GetService<IDuesService>();
            var auditService = serviceProvider.GetService<IAuditService>();

            try
            {
                // Test UserService
                Console.WriteLine("Testing UserService...");
                userService.RegisterUser("John Doe", "john.doe@example.com", "password123");
                var user = userService.GetUserByEmail("john.doe@example.com");
                if (user != null)
                {
                    Console.WriteLine($"User registered: {user.Name}, {user.Email}");
                }
                else
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                // Test CurrencyService
                Console.WriteLine("\nTesting CurrencyService...");
                currencyService.AddCurrency("USD", "US Dollar", 1.0m);
                currencyService.AddCurrency("EUR", "Euro", 0.85m);
                var currencies = currencyService.GetAllCurrencies();
                if (currencies != null)
                {
                    foreach (var currency in currencies)
                    {
                        Console.WriteLine($"Currency: {currency.Code}, {currency.Name}, Exchange Rate: {currency.ExchangeRateToBase}");
                    }
                    if (currencies.Count >= 2)
                    {
                        decimal convertedAmount = currencyService.ConvertAmount(100m, currencies[0].CurrencyId, currencies[1].CurrencyId);
                        Console.WriteLine($"Converted 100 USD to EUR: {convertedAmount}");
                    }
                }

                // Test CategoryService
                Console.WriteLine("\nTesting CategoryService...");
                categoryService.CreateCategory(user.UserId, "Groceries", "Expenses for groceries");
                var categories = categoryService.GetCategoriesByUserId(user.UserId);
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"Category: {category.CategoryName}, {category.CategoryDescription}");
                    }
                }

                // Test TagService
                Console.WriteLine("\nTesting TagService...");
                tagService.CreateTag(user.UserId, "Essential");
                tagService.CreateTag(user.UserId, "Monthly");
                var tags = tagService.GetTagsByUserId(user.UserId);
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        Console.WriteLine($"Tag: {tag.TagName}");
                    }
                }

                // Test ExpenseService
                Console.WriteLine("\nTesting ExpenseService...");
                if (categories?.Count > 0 && currencies?.Count > 0 && tags?.Count > 0)
                {
                    expenseService.AddExpense(user.UserId, "Milk", 3.99m, 2, categories[0].ExpenseCategoryId, currencies[0].CurrencyId, DateTime.Now, new List<string> { tags[0].TagId });
                    var expenses = expenseService.GetExpensesByUserId(user.UserId);
                    if (expenses != null)
                    {
                        foreach (var expense in expenses)
                        {
                            Console.WriteLine($"Expense: {expense.ItemName}, Total Cost: {expense.TotalCost}");
                        }
                    }
                }

                // Test IncomeService
                Console.WriteLine("\nTesting IncomeService...");
                if (currencies?.Count > 0 && tags?.Count > 1)
                {
                    incomeService.AddIncome(user.UserId, "Salary", 5000m, currencies[0].CurrencyId, DateTime.Now, new List<string> { tags[1].TagId });
                    var incomes = incomeService.GetIncomesByUserId(user.UserId);
                    if (incomes != null)
                    {
                        foreach (var income in incomes)
                        {
                            Console.WriteLine($"Income: {income.Source}, Amount: {income.Amount}");
                        }
                    }
                }

                // Test BudgetService
                Console.WriteLine("\nTesting BudgetService...");
                if (categories?.Count > 0)
                {
                    budgetService.SetBudgetGoal(user.UserId, categories[0].ExpenseCategoryId, 50m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                    budgetService.CheckAndCreateBudgetAlert(user.UserId, categories[0].ExpenseCategoryId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                }

                // Test ReportService
                Console.WriteLine("\nTesting ReportService...");
                var (totalExpenses, totalIncome) = reportService.GetFinancialSummary(user.UserId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                Console.WriteLine($"Financial Summary: Total Expenses = {totalExpenses}, Total Income = {totalIncome}");
                var expensesByCategory = reportService.GetExpensesByCategory(user.UserId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                if (expensesByCategory != null)
                {
                    foreach (var kvp in expensesByCategory)
                    {
                        Console.WriteLine($"Category {kvp.Key}: {kvp.Value}");
                    }
                }

                // Test ReminderService
                Console.WriteLine("\nTesting ReminderService...");
                reminderService.CreateReminder(user.UserId, "Pay rent", DateTime.Now.AddDays(1));
                var reminders = reminderService.GetUpcomingReminders(user.UserId, DateTime.Now.AddDays(2));
                if (reminders != null)
                {
                    foreach (var reminder in reminders)
                    {
                        Console.WriteLine($"Reminder: {reminder.Description}, Due: {reminder.DueDate}");
                    }
                }

                // Test RecurringTransactionService
                Console.WriteLine("\nTesting RecurringTransactionService...");
                if (categories?.Count > 0 && currencies?.Count > 0)
                {
                    recurringTransactionService.CreateRecurringTransaction(user.UserId, "Internet Bill", 50m, categories[0].ExpenseCategoryId, currencies[0].CurrencyId, DateTime.Now.AddDays(-1), null, "Monthly");
                    recurringTransactionService.ProcessRecurringTransactions(user.UserId, DateTime.Now);
                    var recurringTransactions = recurringTransactionService.GetRecurringTransactionsByUserId(user.UserId);
                    if (recurringTransactions != null)
                    {
                        foreach (var rt in recurringTransactions)
                        {
                            Console.WriteLine($"Recurring Transaction: {rt.Description}, Amount: {rt.Amount}");
                        }
                    }
                }

                // Test AlertService
                Console.WriteLine("\nTesting AlertService...");
                var pendingAlerts = alertService.GetPendingAlerts(user.UserId); // Assuming IAlertService has this method
                if (pendingAlerts != null)
                {
                    foreach (var alert in pendingAlerts)
                    {
                        Console.WriteLine($"Alert: {alert.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Dispose of the service provider to release resources
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder => builder.AddConsole());

            // Register services (example, replace with actual implementations)
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<IExpenseService, ExpenseService>();
            services.AddSingleton<IIncomeService, IncomeService>();
            services.AddSingleton<ITagService, TagService>();
            services.AddSingleton<IBudgetService, BudgetService>();
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<IReminderService, ReminderService>();
            services.AddSingleton<IRecurringTransactionService, RecurringTransactionService>();
            services.AddSingleton<IAlertService, AlertService>();
            services.AddSingleton<IDuesService, DuesService>();
            services.AddSingleton<IAuditService, AuditService>();

            // Add repositories or other dependencies as needed
            // services.AddSingleton<IUserRepository, UserRepository>();
            // services.AddDbContext<YourDbContext>(options => ...);

            return services.BuildServiceProvider();
        }
    }
}