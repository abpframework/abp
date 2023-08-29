namespace Volo.Abp.Data;

public class AbpRepositoryIsReadOnlyException : AbpException
{
    /// <summary>
    /// Creates a new <see cref="AbpRepositoryIsReadOnlyException"/> object.
    /// </summary>
    public AbpRepositoryIsReadOnlyException()
    {

    }

    /// <summary>
    /// Creates a new <see cref="AbpRepositoryIsReadOnlyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public AbpRepositoryIsReadOnlyException(string message)
        : base(message)
    {

    }
}
