using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class BudgetManagementContext : DbContext
    {
        public BudgetManagementContext(DbContextOptions<BudgetManagementContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=NASIM\MSSQLSERVER01;Database=BudgetManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Incomes)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Dues)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.BudgetAlerts)
                .WithOne(ba => ba.User)
                .HasForeignKey(ba => ba.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reminders)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RecurringTransactions)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tags)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.BudgetGoals)
                .WithOne(bg => bg.User)
                .HasForeignKey(bg => bg.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PreferredCurrency)
                .WithMany()
                .HasForeignKey(u => u.PreferredCurrencyId);

            // ExpenseCategory relationships
            modelBuilder.Entity<ExpenseCategory>()
                .HasMany(ec => ec.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseCategory>()
                .HasMany(ec => ec.RecurringTransactions)
                .WithOne(rt => rt.Category)
                .HasForeignKey(rt => rt.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseCategory>()
                .HasMany(ec => ec.BudgetGoals)
                .WithOne(bg => bg.Category)
                .HasForeignKey(bg => bg.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // TrackExpense and TrackIncome relationships
            modelBuilder.Entity<TrackExpense>()
                .HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrackIncome>()
                .HasOne(i => i.Currency)
                .WithMany()
                .HasForeignKey(i => i.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tag and TransactionTag relationships
            modelBuilder.Entity<TransactionTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TransactionTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionTag>()
                .HasOne(tt => tt.Expense)
                .WithMany(e => e.TransactionTags)
                .HasForeignKey(tt => tt.TrackExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionTag>()
                .HasOne(tt => tt.Income)
                .WithMany(i => i.TransactionTags)
                .HasForeignKey(tt => tt.TrackIncomeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ExpenseCategory>()
                .HasIndex(ec => new { ec.UserId, ec.CategoryName })
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(t => new { t.UserId, t.TagName })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}