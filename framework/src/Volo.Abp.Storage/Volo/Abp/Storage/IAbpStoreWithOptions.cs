using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStoreWithOptions<TOptions> : IAbpStore
        where TOptions : class, IAbpStoreOptions, new()
    {
    }
}