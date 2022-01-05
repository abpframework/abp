namespace Volo.Abp.Logging;

public interface IInitLoggerFactory
{
    IInitLogger<T> Create<T>();
}
