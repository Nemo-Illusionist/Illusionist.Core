using System;
using System.Linq;
using Illusionist.DataProvider.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.DataProvider.Ef.Extension
{
    public static class InsertSystemPropertiesExtensions
    {
        private static void OnSavingChanges(object sender, SavingChangesEventArgs _)
        {
            if (sender is not DbContext context) return;
            
            var now = DateTime.UtcNow;
            var entries = context.ChangeTracker.Entries()
                .Where(static e => e.State is EntityState.Modified or EntityState.Added);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added && entityEntry.Entity is ICreatedUtc createdEntity)
                {
                    createdEntity.CreatedUtc = now;
                }

                if (entityEntry.Entity is IUpdatedUtc updatedEntity)
                {
                    updatedEntity.UpdatedUtc = now;
                }
            }
        }
    }
}