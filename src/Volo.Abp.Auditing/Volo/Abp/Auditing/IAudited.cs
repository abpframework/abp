namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface can be implemented to add standard auditing properties to a class.
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {

    }

    /// <summary>
    /// Extends <see cref="IAudited"/> to add user navigation properties.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IAudited<TUser> : IAudited, ICreationAudited<TUser>, IModificationAudited<TUser>
    {

    }
}