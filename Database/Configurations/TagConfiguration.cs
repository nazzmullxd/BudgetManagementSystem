using Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.TagId);

            builder.Property(t => t.TagId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(t => t.TagName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(100);

            builder.Property(t => t.UserId)
                .HasMaxLength(36)
                .IsRequired();

            // Relationships
            builder.HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(t => new { t.UserId, t.TagName })
                .IsUnique()
                .HasDatabaseName("IX_Tags_UserId_TagName");
        }
    }
}