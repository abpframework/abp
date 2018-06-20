using System;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface can be implemented to store modification information (who and when modified lastly).
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IModificationAudited"/> interface for a user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IModificationAudited<TUser> : IModificationAudited
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        TUser LastModifier { get; set; }
    }
}