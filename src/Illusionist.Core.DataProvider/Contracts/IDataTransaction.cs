using System;

namespace Illusionist.Core.DataProvider.Contracts
{
    public interface IDataTransaction : IDisposable
    {
        /// <summary>
        /// Indicates that all operations within the scope are completed successfully.
        /// </summary>
        void Complete();
    }
}