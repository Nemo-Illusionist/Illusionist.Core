using System.Transactions;
using FluentAssertions;
using Illusionist.DataProvider.Providers;
using NUnit.Framework;

namespace Illusionist.DataProvider.Tests
{
    public class DataTransactionScopeProviderTests
    {
        private readonly DataTransactionScopeProvider _dataTransactionScopeProvider;

        public DataTransactionScopeProviderTests()
        {
            _dataTransactionScopeProvider = new DataTransactionScopeProvider();
        }

        [Test]
        public void TransactionTest()
        {
            var dataTransaction = _dataTransactionScopeProvider.BeginTransaction();

            dataTransaction.Should().NotBeNull();
            var currentLevel = Transaction.Current?.IsolationLevel;
            currentLevel.Should().NotBeNull();
            currentLevel.Should().Be(IsolationLevel.ReadCommitted);
        }

        [TestCase(IsolationLevel.Serializable)]
        [TestCase(IsolationLevel.RepeatableRead)]
        [TestCase(IsolationLevel.ReadCommitted)]
        [TestCase(IsolationLevel.ReadUncommitted)]
        [TestCase(IsolationLevel.Snapshot)]
        [TestCase(IsolationLevel.Chaos)]
        public void NotCurrentTransactionTest(IsolationLevel isolationLevel)
        {
            var startLevel = Transaction.Current?.IsolationLevel;
            startLevel.Should().BeNull();

            var dataTransaction = _dataTransactionScopeProvider.BeginTransaction(isolationLevel);

            dataTransaction.Should().NotBeNull();
            var currentLevel = Transaction.Current?.IsolationLevel;
            currentLevel.Should().NotBeNull();
            currentLevel.Should().Be(isolationLevel);
        }
    }
}