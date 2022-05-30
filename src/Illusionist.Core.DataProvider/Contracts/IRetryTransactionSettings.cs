using System;

namespace Illusionist.Core.DataProvider.Contracts
{
    public interface IRetryTransactionSettings
    {
        /// <summary>
        /// Delay time before retry transaction.
        /// </summary>
        TimeSpan RetryDelay { get; }
    }
}