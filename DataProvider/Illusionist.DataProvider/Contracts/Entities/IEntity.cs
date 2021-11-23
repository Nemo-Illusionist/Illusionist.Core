using System;

namespace Illusionist.DataProvider.Contracts.Entities
{
    public interface IEntity<out TKey>
        where TKey : IComparable
    {
        /// <summary>
        /// Unique identifier of the object.
        /// </summary>
        TKey Id { get; }
    }
}
