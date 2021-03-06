using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Illusionist.Core.DataProvider.Contracts;
using Illusionist.Core.DataProvider.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.Core.DataProvider.Ef.Provider
{
    public class EfDataProvider : EfRoDataProvider, IDataProvider
    {
        private readonly IDataExceptionManager _exceptionManager;

        public EfDataProvider(DbContext connection, IDataExceptionManager exceptionManager)
            : base(connection)
        {
            _exceptionManager = exceptionManager ?? throw new ArgumentNullException(nameof(exceptionManager));
        }

        /// <inheritdoc />
        public virtual Task<T> Insert<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    var entityEntry = await dbContext.Set<T>()
                        .AddAsync(state.entity, state.cancellationToken)
                        .ConfigureAwait(false);
                    await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                    return entityEntry.Entity;
                },
                (entity, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task BatchInsert<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    foreach (var entity in state.entities)
                    {
                       await dbContext.Set<T>().AddAsync(entity, state.cancellationToken).ConfigureAwait(false);
                    }

                    return await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                },
                (entities, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task<T> Update<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                static async (dbContext, state) =>
                {
                    var entityEntry = dbContext.Update(state.entity);
                    await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                    return entityEntry.Entity;
                },
                (entity, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task BatchUpdate<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                static(dbContext, state) =>
                {
                    dbContext.UpdateRange(state.entities);
                    return dbContext.SaveChangesAsync(state.cancellationToken);
                },
                (entities, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task Delete<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                static(dbContext, state) =>
                {
                    dbContext.Set<T>().Remove(state.entity);
                    return dbContext.SaveChangesAsync(state.cancellationToken);
                },
                (entity, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task BatchDelete<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(
                static(dbContext, state) =>
                {
                    dbContext.Set<T>().RemoveRange(state.entities);
                    return dbContext.SaveChangesAsync(state.cancellationToken);
                },
                (entities, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task DeleteById<T, TKey>(TKey id, CancellationToken cancellationToken = default)
            where T : class, IEntity<TKey>
            where TKey : IComparable
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    var entity = await dbContext.Set<T>()
                        .Where(t => state.id.Equals(t.Id))
                        .SingleAsync(state.cancellationToken)
                        .ConfigureAwait(false);
                    dbContext.Set<T>().Remove(entity);
                    return await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                },
                (id, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task BatchDeleteByIds<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
            where T : class, IEntity<TKey>
            where TKey : IComparable
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    var entity = await dbContext.Set<T>()
                        .Where(t => state.ids.Contains(t.Id))
                        .ToArrayAsync(state.cancellationToken)
                        .ConfigureAwait(false);
                    dbContext.Set<T>().RemoveRange(entity);
                    return await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                },
                (ids, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task SetDelete<T, TKey>(TKey id, CancellationToken cancellationToken = default)
            where T : class, IDeletable, IEntity<TKey>
            where TKey : IComparable
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    var entity = await dbContext.Set<T>()
                        .Where(t => state.id.Equals(t.Id))
                        .SingleAsync(state.cancellationToken)
                        .ConfigureAwait(false);

                    entity.DeletedUtc = DateTime.UtcNow;
                    dbContext.Update(entity);

                    return await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                },
                (id, cancellationToken));
        }

        /// <inheritdoc />
        public virtual Task BatchSetDelete<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
            where T : class, IDeletable, IEntity<TKey>
            where TKey : IComparable
        {
            return ExecuteCommand(
                async static(dbContext, state) =>
                {
                    object[] entities = await dbContext.Set<T>()
                        .Where(t => state.ids.Contains(t.Id))
                        .ToArrayAsync(state.cancellationToken)
                        .ConfigureAwait(false);

                    dbContext.UpdateRange(entities);
                    return await dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                },
                (ids, cancellationToken));
        }

        private async Task<T> ExecuteCommand<T, TState>(Func<DbContext, TState, Task<T>> func, TState state)
        {
            try
            {
                return await func(DbContext, state).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw _exceptionManager.Normalize(exception);
            }
        }
    }
}