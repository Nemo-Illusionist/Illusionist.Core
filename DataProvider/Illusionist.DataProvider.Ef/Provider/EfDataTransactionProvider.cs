using System;
using System.Transactions;
using Illusionist.DataProvider.Contracts;
using Illusionist.DataProvider.Ef.Transactions;
using Illusionist.DataProvider.Providers;
using Microsoft.EntityFrameworkCore;

namespace Illusionist.DataProvider.Ef.Provider
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