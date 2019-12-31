using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementStore : IFeatureManagementStore, ITransientDependency
    {
        protected IDistributedCache<FeatureValueCacheItem> Cache { get; }
        protected IFeatureValueRepository FeatureValueRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public FeatureManagementStore(
            IFeatureValueRepository featureValueRepository,
            IGuidGenerator guidGenerator,
            IDistributedCache<FeatureValueCacheItem> cache)
        {
            FeatureValueRepository = featureValueRepository;
            GuidGenerator = guidGenerator;
            Cache = cache;
        }

        public async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var cacheItem = await GetCacheItemAsync(name, providerName, providerKey).ConfigureAwait(false);
            return cacheItem.Value;
        }

        public async Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);
            if (featureValue == null)
            {
                featureValue = new FeatureValue(GuidGenerator.Create(), name, value, providerName, providerKey);
                await FeatureValueRepository.InsertAsync(featureValue).ConfigureAwait(false);
            }
            else
            {
                featureValue.Value = value;
                await FeatureValueRepository.UpdateAsync(featureValue).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);
            if (featureValue != null)
            {
                await FeatureValueRepository.DeleteAsync(featureValue).ConfigureAwait(false);
            }
        }

        protected virtual async Task<FeatureValueCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
            var cacheItem = await Cache.GetAsync(cacheKey).ConfigureAwait(false);

            if (cacheItem != null)
            {
                return cacheItem;
            }

            var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);

            cacheItem = new FeatureValueCacheItem(featureValue?.Value);

            await Cache.SetAsync(
                cacheKey,
                cacheItem
            ).ConfigureAwait(false);

            return cacheItem;
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}