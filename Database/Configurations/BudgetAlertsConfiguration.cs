using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class BudgetAlertsConfiguration : IEntityTypeConfiguration<BudgetAlerts>
    {
        public void Configure(EntityTypeBuilder<BudgetAlerts> builder)
        {
            builder.HasKey(ba => ba.BudgetAlertsId);

            builder.Property(ba => ba.BudgetAlertsId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(ba => ba.DailyLimit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ba => ba.WeeklyLimit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ba => ba.MonthlyLimit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ba => ba.ThresholdPercentage)
                .HasColumnType("decimal(5,2)")
                .IsRequired()
                .HasDefaultValue(0.9M);

            builder.Property(ba => ba.UserId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(ba => ba.User)
                .WithMany(u => u.BudgetAlerts)
                .HasForeignKey(ba => ba.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}