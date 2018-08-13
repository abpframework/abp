namespace Volo.Abp.Storage
{
    public abstract class AbpStoreBase
    {
        public AbpStoreBase(string storeName, IAbpStorageFactory storageFactory)
        {
            Store = storageFactory.GetStore(storeName);
        }

        public IAbpStore Store { get; }
    }
}