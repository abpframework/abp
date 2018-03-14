using System;

namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface can be implemented to store deletion information (who delete and when deleted).
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// Id of the deleter user.
        /// </summary>
        Guid? DeleterId { get; set; }
    }

    /// <summary>
    /// Extends <see cref="IDeletionAudited"/> to add user navigation propery.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IDeletionAudited<TUser> : IDeletionAudited
    {
        /// <summary>
        /// Reference to the deleter user.
        /// </summary>
        TUser Deleter { get; set; }
    }
}