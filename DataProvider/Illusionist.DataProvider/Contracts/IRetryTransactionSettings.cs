using System;

namespace Illusionist.DataProvider.Contracts
{
    public interface IRetryTransactionSettings
    {
        /// <summary>
        /// Delay time before retry transaction.
        /// </summary>
        TimeSpan RetryDelay { get; }
    }
}