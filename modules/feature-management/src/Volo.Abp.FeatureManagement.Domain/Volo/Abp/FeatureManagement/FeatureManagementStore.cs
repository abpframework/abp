using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Volo.Abp.FeatureManagement;

public class FeatureManagementStore : IFeatureManagementStore, ITransientDependency
{
    public ILogger<FeatureManagementStore> Logger { get; set; }

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
        Logger = NullLogger<FeatureManagementStore>.Instance;
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
        var featureValue = await FindAndDeleteDuplicatesAsync(name, providerName, providerKey);
        if (featureValue == null)
        {
            featureValue = new FeatureValue(GuidGenerator.Create(), name, value, providerName, providerKey);
            await FeatureValueRepository.InsertAsync(featureValue, true);
        }
        else
        {
            featureValue.Value = value;
            await FeatureValueRepository.UpdateAsync(featureValue, true);
        }

        await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new FeatureValueCacheItem(featureValue?.Value), considerUow: true);
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(string name, string providerName, string providerKey)
    {
        var featureValues = await FeatureValueRepository.FindAllAsync(name, providerName, providerKey);
        foreach (var featureValue in featureValues)
        {
            await FeatureValueRepository.DeleteAsync(featureValue, true);
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
        var featureDefinitions = await FeatureDefinitionManager.GetAllAsync();
        var featuresDictionary = await CreateFeatureValueDictionaryAsync(
            await FeatureValueRepository.GetListAsync(providerName, providerKey),
            providerName,
            providerKey);

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

    protected virtual async Task<FeatureValue> FindAndDeleteDuplicatesAsync(string name, string providerName, string providerKey)
    {
        var featureValues = await FeatureValueRepository.FindAllAsync(name, providerName, providerKey);
        if (featureValues.Count <= 1)
        {
            return featureValues.FirstOrDefault();
        }

        var featureValue = await FeatureValueRepository.FindAsync(name, providerName, providerKey);
        if (featureValue == null)
        {
            return null;
        }

        foreach (var duplicate in featureValues.Where(x => x.Id != featureValue.Id))
        {
            await FeatureValueRepository.DeleteAsync(duplicate, true);
        }

        return featureValue;
    }

    protected virtual async Task<Dictionary<string, string>> CreateFeatureValueDictionaryAsync(
        List<FeatureValue> featureValues,
        string providerName,
        string providerKey)
    {
        var featuresDictionary = new Dictionary<string, string>();

        foreach (var featureValueGroup in featureValues.GroupBy(s => s.Name))
        {
            if (featureValueGroup.Count() == 1)
            {
                featuresDictionary[featureValueGroup.Key] = featureValueGroup.First().Value;
                continue;
            }

            // FindAsync returns the same record that SetAsync updates, so reads and writes agree until the duplicates are deleted.
            Logger.LogWarning(
                "Found {Count} feature value records for Name = {Name}, ProviderName = {ProviderName}, ProviderKey = {ProviderKey}. The duplicates will be deleted the next time the feature value is saved.",
                featureValueGroup.Count(), featureValueGroup.Key, providerName, providerKey);

            var featureValue = await FeatureValueRepository.FindAsync(featureValueGroup.Key, providerName, providerKey);
            featuresDictionary[featureValueGroup.Key] = (featureValue ?? featureValueGroup.First()).Value;
        }

        return featuresDictionary;
    }

    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
    {
        return FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
    }
}
