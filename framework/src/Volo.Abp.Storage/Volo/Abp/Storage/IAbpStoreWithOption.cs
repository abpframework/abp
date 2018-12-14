using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStoreWithOption<TOptions> : IAbpStore
        where TOptions : class, IAbpStoreOptions, new()
    {
    }
}