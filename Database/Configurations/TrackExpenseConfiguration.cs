using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class TrackExpenseConfiguration : IEntityTypeConfiguration<TrackExpense>
    {
        public void Configure(EntityTypeBuilder<TrackExpense> builder)
        {
            builder.HasKey(e => e.TrackExpenseId);

            builder.Property(e => e.TrackExpenseId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.ItemName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.ExpenseCategoryId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.CurrencyId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(e => e.ItemPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.Quantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.TransactionDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // TotalCost is a computed property (NotMapped)

            // Relationships
            builder.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => new { e.UserId, e.TransactionDate })
                .HasDatabaseName("IX_TrackExpenses_UserId_TransactionDate");

            builder.HasIndex(e => e.ExpenseCategoryId)
                .HasDatabaseName("IX_TrackExpenses_CategoryId");
        }
    }
}