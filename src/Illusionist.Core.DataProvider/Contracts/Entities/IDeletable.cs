using System;

namespace Illusionist.Core.DataProvider.Contracts.Entities
{
    public interface IDeletable
    {
        /// <summary>
        /// Date of deletion of the object.
        /// </summary>
        /// <remark> <see langword="null"/> means object is not yet deleted. </remark>
        DateTime? DeletedUtc { get; set; }
    }
}