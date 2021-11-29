using System;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// A standard interface to add CreationTime property.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time.
        /// </summary>
        DateTime CreationTime { get; }
    }
}
