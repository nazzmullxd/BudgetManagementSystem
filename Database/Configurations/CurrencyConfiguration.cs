using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(c => c.CurrencyId);

            builder.Property(c => c.CurrencyId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(c => c.Code)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ExchangeRateToBase)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(1.0M);

            // Indexes
            builder.HasIndex(c => c.Code)
                .IsUnique()
                .HasDatabaseName("IX_Currencies_Code");
        }
    }
}