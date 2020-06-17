using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementStoreCache : UnitOfWorkCacheWithFallback<SettingCacheItem>, ITransientDependency
    {
        public override string Name => "SettingManagementStoreUnitOfWorkCache";

        protected IDistributedCache<SettingCacheItem> Cache { get; }

        public SettingManagementStoreCache(IUnitOfWorkManager unitOfWorkManager, IDistributedCache<SettingCacheItem> cache)
            : base(unitOfWorkManager)
        {
            Cache = cache;
        }

        public override async Task<SettingCacheItem> GetFallbackCacheItem(string key)
        {
            return await Cache.GetAsync(key);
        }

        public override async Task<SettingCacheItem> SetFallbackCacheItem(string key, SettingCacheItem item)
        {
            await Cache.SetAsync(key, item);
            return item;
        }
    }
}
