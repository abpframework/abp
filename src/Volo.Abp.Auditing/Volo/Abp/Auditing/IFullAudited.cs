namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface adds <see cref="IDeletionAudited"/> to <see cref="IAudited"/>.
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {
        
    }

    /// <summary>
    /// Adds user navigation properties to <see cref="IFullAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IFullAudited<TUser> : IAudited<TUser>, IFullAudited, IDeletionAudited<TUser>
    {

    }
}