using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class BudgetManagementContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=NASIM\MSSQLSERVER01;Trusted_Connection=True;TrustServerCertificate=True;ConnectRetryCount=0");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrackIncome> TrackIncomes { get; set; }
        public DbSet<TrackExpense> TrackExpenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<DuesManagement> DuesManagements { get; set; }
        public DbSet<BudgetAlerts> BudgetAlerts { get; set; }
    }
}
