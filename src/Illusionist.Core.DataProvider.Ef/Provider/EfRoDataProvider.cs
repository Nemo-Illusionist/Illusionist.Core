using System;
using System.Linq;
using System.Linq.Expressions;
using Illusionist.Core.DataProvider.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.Core.DataProvider.Ef.Provider
{
    public class EfRoDataProvider : EfDataTransactionProvider, IRoDataProvider
    {
        public EfRoDataProvider(DbContext connection) 
            : base(connection)
        {
        }

        /// <inheritdoc />
        public virtual IQueryable<T> GetQueryable<T>() 
            where T : class
        {
            return DbContext.Set<T>().AsQueryable().AsNoTracking();
        }

        /// <inheritdoc />
        public virtual IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> predicate) 
            where T : class
        {
            return DbContext.Set<T>().AsQueryable().Where(predicate).AsNoTracking();
        }
    }
}