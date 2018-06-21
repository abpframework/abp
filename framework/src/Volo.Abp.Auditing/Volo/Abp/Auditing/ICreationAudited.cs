namespace Volo.Abp.Auditing
{
    /// <summary>
    /// This interface can be implemented to store creation information (who and when created).
    /// </summary>
    public interface ICreationAudited : IHasCreationTime, IMayHaveCreator
    {

    }

    /// <summary>
    /// Adds navigation property (object reference) to <see cref="ICreationAudited"/> interface.
    /// </summary>
    /// <typeparam name="TCreator">Type of the user</typeparam>
    public interface ICreationAudited<TCreator> : ICreationAudited, IMayHaveCreator<TCreator>
    {

    }
}