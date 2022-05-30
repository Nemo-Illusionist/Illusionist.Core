using System;
using System.Transactions;
using Illusionist.Core.DataProvider.Contracts;
using Illusionist.Core.DataProvider.Ef.Transactions;
using Illusionist.Core.DataProvider.Providers;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.Core.DataProvider.Ef.Provider
{
    public class EfDataTransactionProvider : DataTransactionScopeProvider
    {
        protected DbContext DbContext { get; }

        public EfDataTransactionProvider(DbContext connection)
        {
            DbContext = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public override IDataTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            var dataTransaction = base.BeginTransaction(isolationLevel);
            return new EfDecoratorDataTransaction(dataTransaction, DbContext);
        }
    }
}