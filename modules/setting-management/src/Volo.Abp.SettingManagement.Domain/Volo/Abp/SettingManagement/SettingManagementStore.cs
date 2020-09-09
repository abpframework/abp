using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement
{
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

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
