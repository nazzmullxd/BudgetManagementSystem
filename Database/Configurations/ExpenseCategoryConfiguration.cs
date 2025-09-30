using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.HasKey(ec => ec.ExpenseCategoryId);

            builder.Property(ec => ec.ExpenseCategoryId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(ec => ec.CategoryName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(ec => ec.CategoryDescription)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ec => ec.UserId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(ec => ec.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(ec => ec.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ec => ec.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ec => ec.RecurringTransactions)
                .WithOne(rt => rt.Category)
                .HasForeignKey(rt => rt.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ec => ec.BudgetGoals)
                .WithOne(bg => bg.Category)
                .HasForeignKey(bg => bg.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(ec => new { ec.UserId, ec.CategoryName })
                .IsUnique()
                .HasDatabaseName("IX_ExpenseCategories_UserId_CategoryName");
        }
    }
}