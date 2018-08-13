using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage
{
    public interface IAbpStorageFactory
    {
        IAbpStore GetStore(string storeName, IAbpStoreOptions configuration);

        IAbpStore GetStore(string storeName);

        IAbpStore GetScopedStore(string storeName, params object[] args);

        bool TryGetStore(string storeName, out IAbpStore store);

        bool TryGetStore(string storeName, out IAbpStore store, string provider);
    }
}