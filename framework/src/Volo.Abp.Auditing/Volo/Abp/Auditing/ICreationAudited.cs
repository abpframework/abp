using System;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface can be implemented to store creation information (who and when created).
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// Id of the creator user.
        /// </summary>
        Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="ICreationAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
    {
        /// <summary>
        /// Reference to the creator user.
        /// </summary>
        TUser Creator { get; set; }
    }
}