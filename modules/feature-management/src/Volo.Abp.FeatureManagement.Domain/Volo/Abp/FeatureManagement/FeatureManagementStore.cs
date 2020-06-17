using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementStore : IFeatureManagementStore, ITransientDependency
    {
        protected IFeatureValueRepository FeatureValueRepository { get; }
        protected FeatureManagementStoreCache Cache { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public FeatureManagementStore(
            IFeatureValueRepository featureValueRepository,
            FeatureManagementStoreCache featureManagementStoreCache,
            IGuidGenerator guidGenerator)
        {
            FeatureValueRepository = featureValueRepository;
            Cache = featureManagementStoreCache;
            GuidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public virtual async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var cacheItem = await Cache.GetOrAddAsync(CalculateCacheKey(name, providerName, providerKey),
                async () => new FeatureValueCacheItem((await FeatureValueRepository.FindAsync(name, providerName, providerKey))?.Value));
            return cacheItem.Value;
        }

        [UnitOfWork]
        public virtual async Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey);
            if (featureValue == null)
            {
                featureValue = new FeatureValue(GuidGenerator.Create(), name, value, providerName, providerKey);
                await FeatureValueRepository.InsertAsync(featureValue);
            }
            else
            {
                featureValue.Value = value;
                await FeatureValueRepository.UpdateAsync(featureValue);
            }

            await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new FeatureValueCacheItem(featureValue.Value));
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var featureValues = await FeatureValueRepository.FindAllAsync(name, providerName, providerKey);
            foreach (var featureValue in featureValues)
            {
                await FeatureValueRepository.DeleteAsync(featureValue);
                await Cache.RemoveAsync(CalculateCacheKey(name, providerName, providerKey));
            }
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
