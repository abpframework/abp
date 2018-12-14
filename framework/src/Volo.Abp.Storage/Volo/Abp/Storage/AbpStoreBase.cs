namespace Volo.Abp.Storage
{
    public abstract class AbpStoreBase
    {
        private readonly IAbpStore _store;

        public AbpStoreBase(string storeName, IAbpStorageFactory storageFactory)
        {
            _store = storageFactory.GetStore(storeName);
        }

        public IAbpStore Store => _store;
    }
}
