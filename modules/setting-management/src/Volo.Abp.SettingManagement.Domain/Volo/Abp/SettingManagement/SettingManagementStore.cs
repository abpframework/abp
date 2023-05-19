using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement;

public class SettingManagementStore : ISettingManagementStore, ITransientDependency
{
    protected IDistributedCache<SettingCacheItem> Cache { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }
    protected ISettingRepository SettingRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public SettingManagementStore(
        ISettingRepository settingRepository,
        IGuidGenerator guidGenerator,
        IDistributedCache<SettingCacheItem> cache,
        ISettingDefinitionManager settingDefinitionManager)
    {
        SettingRepository = settingRepository;
        GuidGenerator = guidGenerator;
        Cache = cache;
        SettingDefinitionManager = settingDefinitionManager;
    }

    [UnitOfWork]
    public virtual async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
    {
        return (await GetCacheItemAsync(name, providerName, providerKey)).Value;
    }

    [UnitOfWork]
    public virtual async Task SetAsync(string name, string value, string providerName, string providerKey)
    {
        var setting = await SettingRepository.FindAsync(name, providerName, providerKey);
        if (setting == null)
        {
            setting = new Setting(GuidGenerator.Create(), name, value, providerName, providerKey);
            await SettingRepository.InsertAsync(setting);
        }
        else
        {
            setting.Value = value;
            await SettingRepository.UpdateAsync(setting);
        }

        await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new SettingCacheItem(setting?.Value), considerUow: true);
    }

    public virtual async Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
    {
        var settings = await SettingRepository.GetListAsync(providerName, providerKey);
        return settings.Select(s => new SettingValue(s.Name, s.Value)).ToList();
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(string name, string providerName, string providerKey)
    {
        var setting = await SettingRepository.FindAsync(name, providerName, providerKey);
        if (setting != null)
        {
            await SettingRepository.DeleteAsync(setting);
            await Cache.RemoveAsync(CalculateCacheKey(name, providerName, providerKey), considerUow: true);
        }
    }

    protected virtual async Task<SettingCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
    {
        var cacheKey = CalculateCacheKey(name, providerName, providerKey);
        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);

        if (cacheItem != null)
        {
            return cacheItem;
        }

        cacheItem = new SettingCacheItem(null);

        await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);

        return cacheItem;
    }

    private async Task SetCacheItemsAsync(
        string providerName,
        string providerKey,
        string currentName,
        SettingCacheItem currentCacheItem)
    {
        var settingDefinitions = SettingDefinitionManager.GetAll();
        var settingsDictionary = (await SettingRepository.GetListAsync(providerName, providerKey))
            .ToDictionary(s => s.Name, s => s.Value);

        var cacheItems = new List<KeyValuePair<string, SettingCacheItem>>();

        foreach (var settingDefinition in settingDefinitions)
        {
            var settingValue = settingsDictionary.GetOrDefault(settingDefinition.Name);

            cacheItems.Add(
                new KeyValuePair<string, SettingCacheItem>(
                    CalculateCacheKey(settingDefinition.Name, providerName, providerKey),
                    new SettingCacheItem(settingValue)
                )
            );

            if (settingDefinition.Name == currentName)
            {
                currentCacheItem.Value = settingValue;
            }
        }

        await Cache.SetManyAsync(cacheItems, considerUow: true);
    }

    [UnitOfWork]
    public async Task<List<SettingValue>> GetListAsync(string[] names, string providerName, string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new List<SettingValue>();

        if (names.Length == 1)
        {
            var name = names.First();
            result.Add(new SettingValue(name, (await GetCacheItemAsync(name, providerName, providerKey)).Value));
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Add(new SettingValue(GetSettingNameFormCacheKeyOrNull(item.Key), item.Value?.Value));
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<string, SettingCacheItem>>> GetCacheItemsAsync(string[] names, string providerName, string providerKey)
    {
        var cacheKeys = names.Select(x => CalculateCacheKey(x, providerName, providerKey)).ToList();

        var cacheItems = (await Cache.GetManyAsync(cacheKeys, considerUow: true)).ToList();

        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems;
        }

        var notCacheKeys = cacheItems.Where(x => x.Value == null).Select(x => x.Key).ToList();

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var result = new List<KeyValuePair<string, SettingCacheItem>>();
        foreach (var key in cacheKeys)
        {
            var item = newCacheItems.FirstOrDefault(x => x.Key == key);
            if (item.Value == null)
            {
                item = cacheItems.FirstOrDefault(x => x.Key == key);
            }

            result.Add(new KeyValuePair<string, SettingCacheItem>(key, item.Value));
        }

        return result;
    }

    private async Task<List<KeyValuePair<string, SettingCacheItem>>> SetCacheItemsAsync(
        string providerName,
        string providerKey,
        List<string> notCacheKeys)
    {
        var settingDefinitions = SettingDefinitionManager.GetAll().Where(x => notCacheKeys.Any(k => GetSettingNameFormCacheKeyOrNull(k) == x.Name));

        var settingsDictionary = (await SettingRepository.GetListAsync(notCacheKeys.Select(GetSettingNameFormCacheKeyOrNull).ToArray(), providerName, providerKey))
            .ToDictionary(s => s.Name, s => s.Value);

        var cacheItems = new List<KeyValuePair<string, SettingCacheItem>>();

        foreach (var settingDefinition in settingDefinitions)
        {
            var settingValue = settingsDictionary.GetOrDefault(settingDefinition.Name);
            cacheItems.Add(
                new KeyValuePair<string, SettingCacheItem>(
                    CalculateCacheKey(settingDefinition.Name, providerName, providerKey),
                    new SettingCacheItem(settingValue)
                )
            );
        }

        await Cache.SetManyAsync(cacheItems, considerUow: true);

        return cacheItems;
    }


    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
    {
        return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
    }

    protected virtual string GetSettingNameFormCacheKeyOrNull(string key)
    {
        //TODO: throw ex when name is null?
        return SettingCacheItem.GetSettingNameFormCacheKeyOrNull(key);
    }
}
