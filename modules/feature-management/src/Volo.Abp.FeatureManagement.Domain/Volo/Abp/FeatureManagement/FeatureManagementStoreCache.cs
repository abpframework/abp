using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementStoreCache : UnitOfWorkCacheWithFallback<FeatureValueCacheItem>, ITransientDependency
    {
        public override string Name => "FeatureManagementStoreUnitOfWorkCache";

        protected IDistributedCache<FeatureValueCacheItem> Cache { get; }

        public FeatureManagementStoreCache(IUnitOfWorkManager unitOfWorkManager, IDistributedCache<FeatureValueCacheItem> cache)
            : base(unitOfWorkManager)
        {
            Cache = cache;
        }

        public override async Task<FeatureValueCacheItem> GetFallbackCacheItem(string key)
        {
            return await Cache.GetAsync(key);
        }

        public override async Task<FeatureValueCacheItem> SetFallbackCacheItem(string key, FeatureValueCacheItem item)
        {
            await Cache.SetAsync(key, item);
            return item;
        }
    }
}
