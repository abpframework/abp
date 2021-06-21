using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementStore : IFeatureManagementStore, ITransientDependency
    {
        protected IDistributedCache<FeatureValueCacheItem> Cache { get; }
        protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
        protected IFeatureValueRepository FeatureValueRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public FeatureManagementStore(
            IFeatureValueRepository featureValueRepository,
            IGuidGenerator guidGenerator,
            IDistributedCache<FeatureValueCacheItem> cache,
            IFeatureDefinitionManager featureDefinitionManager)
        {
            FeatureValueRepository = featureValueRepository;
            GuidGenerator = guidGenerator;
            Cache = cache;
            FeatureDefinitionManager = featureDefinitionManager;
        }

        [UnitOfWork]
        public virtual async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var cacheItem = await GetCacheItemAsync(name, providerName, providerKey);
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

            await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new FeatureValueCacheItem(featureValue?.Value), considerUow: true);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var featureValues = await FeatureValueRepository.FindAllAsync(name, providerName, providerKey);
            foreach (var featureValue in featureValues)
            {
                await FeatureValueRepository.DeleteAsync(featureValue);
                await Cache.RemoveAsync(CalculateCacheKey(name, providerName, providerKey), considerUow: true);
            }
        }

        protected virtual async Task<FeatureValueCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
            var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);

            if (cacheItem != null)
            {
                return cacheItem;
            }

            cacheItem = new FeatureValueCacheItem(null);

            await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);

            return cacheItem;
        }

        private async Task SetCacheItemsAsync(
            string providerName,
            string providerKey,
            string currentName,
            FeatureValueCacheItem currentCacheItem)
        {
            var featureDefinitions = FeatureDefinitionManager.GetAll();
            var featuresDictionary = (await FeatureValueRepository.GetListAsync(providerName, providerKey))
                .ToDictionary(s => s.Name, s => s.Value);

            var cacheItems = new List<KeyValuePair<string, FeatureValueCacheItem>>();

            foreach (var featureDefinition in featureDefinitions)
            {
                var featureValue = featuresDictionary.GetOrDefault(featureDefinition.Name);

                cacheItems.Add(
                    new KeyValuePair<string, FeatureValueCacheItem>(
                        CalculateCacheKey(featureDefinition.Name, providerName, providerKey),
                        new FeatureValueCacheItem(featureValue)
                    )
                );

                if (featureDefinition.Name == currentName)
                {
                    currentCacheItem.Value = featureValue;
                }
            }

            await Cache.SetManyAsync(cacheItems, considerUow: true);
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
