using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStorageProvider
    {
        string Name { get; }

        IAbpStore BuildStore(string storeName);

        IAbpStore BuildStore(string storeName, IAbpStoreOptions storeOptions);

        IAbpStore BuildScopedStore(string storeName, params object[] args);
    }
}
