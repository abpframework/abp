using System;
using JetBrains.Annotations;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// Standard interface for an entity that MUST have a creator of type <typeparamref name="TCreator"/>.
    /// </summary>
    public interface IMustHaveCreator<TCreator> : IMustHaveCreator
    {
        /// <summary>
        /// Reference to the creator.
        /// </summary>
        [NotNull]
        TCreator Creator { get; }
    }

    /// <summary>
    /// Standard interface for an entity that MUST have a creator.
    /// </summary>
    public interface IMustHaveCreator
    {
        /// <summary>
        /// Id of the creator.
        /// </summary>
        Guid CreatorId { get; }
    }
}
