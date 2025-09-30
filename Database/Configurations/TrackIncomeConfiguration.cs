using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class TrackIncomeConfiguration : IEntityTypeConfiguration<TrackIncome>
    {
        public void Configure(EntityTypeBuilder<TrackIncome> builder)
        {
            builder.HasKey(i => i.IncomeId);

            builder.Property(i => i.IncomeId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(i => i.IncomeSource)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.IncomeType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.IncomeDescription)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.IncomeAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.IncomeDate)
                .IsRequired();

            builder.Property(i => i.IncomeTax)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.Frequency)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.UserId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(i => i.CurrencyId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(i => i.User)
                .WithMany(u => u.Incomes)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Currency)
                .WithMany()
                .HasForeignKey(i => i.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(i => new { i.UserId, i.IncomeDate })
                .HasDatabaseName("IX_TrackIncomes_UserId_IncomeDate");
        }
    }
}