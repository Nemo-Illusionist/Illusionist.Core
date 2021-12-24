using System;

namespace Illusionist.DataProvider.Contracts.Entities
{
    public interface ICreatedUtc
    {
        /// <summary>
        /// Creation date of the object.
        /// </summary>
        DateTime CreatedUtc { get; set; }
    }
}