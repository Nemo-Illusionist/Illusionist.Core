using System;
using System.Transactions;
using FluentAssertions;
using Illusionist.DataProvider.Transactions;
using NUnit.Framework;

namespace Illusionist.DataProvider.Tests
{
    public class TransactionScopeToDataTransactionAdapterTests
    {
        [Test]
        public void Test()
        {
            var startLevel = Transaction.Current?.IsolationLevel;
            startLevel.Should().BeNull();

            var transaction = CreateTransaction();

            startLevel = Transaction.Current?.IsolationLevel;
            startLevel.Should().Be(IsolationLevel.ReadCommitted);

            transaction.Complete();

            Assert.Throws<InvalidOperationException>(() => _ = Transaction.Current?.IsolationLevel);

            transaction.Dispose();
            Assert.Throws<ObjectDisposedException>(() => transaction.Complete());
        }

        private static TransactionScopeToDataTransactionAdapter CreateTransaction()
        {
            var txOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                txOptions,
                TransactionScopeAsyncFlowOption.Enabled);

            var transaction = new TransactionScopeToDataTransactionAdapter(transactionScope);
            return transaction;
        }
    }
}