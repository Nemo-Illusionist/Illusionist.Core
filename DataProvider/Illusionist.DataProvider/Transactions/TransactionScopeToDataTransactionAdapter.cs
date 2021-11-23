using System;
using System.Transactions;
using Illusionist.DataProvider.Contracts;

namespace Illusionist.DataProvider.Transactions
{
    public sealed class TransactionScopeToDataTransactionAdapter : IDataTransaction
    {
        private TransactionScope? _transactionScope;

        public TransactionScopeToDataTransactionAdapter(TransactionScope transactionScope)
        {
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        /// <inheritdoc />
        public void Complete()
        {
            if (_transactionScope is null)
            {
                throw new ObjectDisposedException(nameof(_transactionScope));
            }

            _transactionScope.Complete();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _transactionScope?.Dispose();
            _transactionScope = null;
        }
    }
}