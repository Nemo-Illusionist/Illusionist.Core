using System;
using System.Collections.Generic;
using Illusionist.Core.DataProvider.Ef.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.Core.DataProvider.Ef.Contexts
{
    public abstract class ModelDbContext : DbContext
    {
        protected IReadOnlyCollection<Type> ModeTypes { get; }

        protected ModelDbContext(IModelStore modelStore)
        {
            if (modelStore == null) throw new ArgumentNullException(nameof(modelStore));
            
            ModeTypes = modelStore.GetModels();
        }

        protected ModelDbContext(IModelStore modelStore, DbContextOptions options)
            : base(options)
        {
            if (modelStore == null) throw new ArgumentNullException(nameof(modelStore));
            
            ModeTypes = modelStore.GetModels();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
            
            base.OnModelCreating(modelBuilder);

            foreach (var modeType in ModeTypes)
            {
                modelBuilder.Entity(modeType);
            }
        }
    }
}