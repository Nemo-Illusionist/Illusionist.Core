using System;
using System.Transactions;
using Illusionist.Core.DataProvider.Contracts;
using Illusionist.Core.DataProvider.Transactions;

namespace Illusionist.Core.DataProvider.Providers
{
    public class DataTransactionScopeProvider : IDataTransactionProvider
    {
        /// <inheritdoc />
        public virtual IDataTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <inheritdoc />
        public virtual IDataTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            var ambientLevel = Transaction.Current?.IsolationLevel;
            var txOptions = new TransactionOptions
            {
                IsolationLevel = ambientLevel == null
                    ? isolationLevel
                    : (IsolationLevel) Math.Min((int) ambientLevel, (int) isolationLevel)
            };

            var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                txOptions,
                TransactionScopeAsyncFlowOption.Enabled);

            return new TransactionScopeToDataTransactionAdapter(transactionScope);
        }
    }
}