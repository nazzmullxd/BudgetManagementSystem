using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class DuesManagementConfiguration : IEntityTypeConfiguration<DuesManagement>
    {
        public void Configure(EntityTypeBuilder<DuesManagement> builder)
        {
            builder.HasKey(d => d.DueId);

            builder.Property(d => d.DueId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(d => d.Payee)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.TotalDueAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(d => d.PaidAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(d => d.UserId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(d => d.User)
                .WithMany(u => u.Dues)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}