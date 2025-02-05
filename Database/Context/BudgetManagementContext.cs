using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class BudgetManagementContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=BudgetManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrackIncome> TrackIncomes { get; set; }
        public DbSet<TrackExpense> TrackExpenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<DuesManagement> DuesManagements { get; set; }
        public DbSet<BudgetAlerts> BudgetAlerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, keys, indexes, etc.
            base.OnModelCreating(modelBuilder);
        }
    }
}
