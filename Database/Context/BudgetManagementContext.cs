using Database.Extensions;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class BudgetManagementContext : DbContext
    {
        public BudgetManagementContext(DbContextOptions<BudgetManagementContext> options) : base(options)
        {
        }

        public BudgetManagementContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=localhost\MSSQLSERVER02;Database=BudgetManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;",
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null));
                        
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }
        }

        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<TrackIncome> TrackIncomes { get; set; }
        public DbSet<TrackExpense> TrackExpenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<DuesManagement> DuesManagements { get; set; }
        public DbSet<BudgetAlerts> BudgetAlerts { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TransactionTag> TransactionTags { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<BudgetGoal> BudgetGoals { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all entity configurations using the extension method
            modelBuilder.ApplyAllConfigurations();
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added && entry.Entity.GetType().GetProperty("CreatedAt") != null)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}