using Database.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            // Apply all entity configurations from the Configurations folder
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Additional global configurations
            ApplyGlobalConfigurations(modelBuilder);
        }

        private static void ApplyGlobalConfigurations(ModelBuilder modelBuilder)
        {
            // Don't set automatic string lengths - they are handled in entity configurations

            // Set default DateTime behavior
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("datetime2");
                    }
                }
            }

            // Set default decimal precision
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        if (property.GetColumnType() == null)
                        {
                            property.SetColumnType("decimal(18,2)");
                        }
                    }
                }
            }
        }
    }
}