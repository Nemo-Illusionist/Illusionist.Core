using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Illusionist.Core.DataProvider.Ef.Extension
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseValueConverterForDateTimeUtc(this ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            modelBuilder.UseValueConverterForType(dateTimeConverter);
            
            return modelBuilder;
        }
        
        public static ModelBuilder UseValueConverterForDateTimeNullableUtc(this ModelBuilder modelBuilder)
        {
            var dateTimeNullableConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);
            modelBuilder.UseValueConverterForType(dateTimeNullableConverter);
            
            return modelBuilder;
        }

        public static ModelBuilder UseValueConverterForType<TModel, TProvider>(
            this ModelBuilder modelBuilder,
            ValueConverter<TModel, TProvider> converter)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            return UseValueConverterForType(modelBuilder, typeof(TModel), converter);
        }
        
        private static ModelBuilder UseValueConverterForType(
            ModelBuilder modelBuilder,
            Type type,
            ValueConverter converter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == type);

                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(converter);
                }
            }

            return modelBuilder;
        }
    }
}