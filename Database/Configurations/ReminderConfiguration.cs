using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.HasKey(r => r.ReminderId);

            builder.Property(r => r.ReminderId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.UserId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reminders)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
