using System;

namespace Illusionist.DataProvider.Contracts.Entities
{
    public interface IUpdatedUtc
    {
        /// <summary>
        /// Last update date of the object.
        /// </summary>
        DateTime UpdatedUtc { get; set; }
    }
}