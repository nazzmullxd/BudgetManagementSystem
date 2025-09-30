using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class BudgetGoalConfiguration : IEntityTypeConfiguration<BudgetGoal>
    {
        public void Configure(EntityTypeBuilder<BudgetGoal> builder)
        {
            builder.HasKey(bg => bg.BudgetGoalId);

            builder.Property(bg => bg.BudgetGoalId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(bg => bg.GoalName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(bg => bg.TargetAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(bg => bg.CurrentAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(bg => bg.UserId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(bg => bg.ExpenseCategoryId)
                .HasMaxLength(36);

            // Relationships
            builder.HasOne(bg => bg.User)
                .WithMany(u => u.BudgetGoals)
                .HasForeignKey(bg => bg.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bg => bg.Category)
                .WithMany(c => c.BudgetGoals)
                .HasForeignKey(bg => bg.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}