using System;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// A standard interface to add DeletionTime property to a class.
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}