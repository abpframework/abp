using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementStore : ISettingManagementStore, ITransientDependency
    {
        protected IDistributedCache<SettingCacheItem> Cache { get; }
        protected ISettingRepository SettingRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public SettingManagementStore(
            ISettingRepository settingRepository, 
            IGuidGenerator guidGenerator, 
            IDistributedCache<SettingCacheItem> cache)
        {
            SettingRepository = settingRepository;
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
            var setting = await SettingRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);
            if (setting == null)
            {
                setting = new Setting(GuidGenerator.Create(), name, value, providerName, providerKey);
                await SettingRepository.InsertAsync(setting).ConfigureAwait(false);
            }
            else
            {
                setting.Value = value;
                await SettingRepository.UpdateAsync(setting).ConfigureAwait(false);
            }
        }

        public async Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
        {
            var settings = await SettingRepository.GetListAsync(providerName, providerKey).ConfigureAwait(false);
            return settings.Select(s => new SettingValue(s.Name, s.Value)).ToList();
        }

        public async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var setting = await SettingRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);
            if (setting != null)
            {
                await SettingRepository.DeleteAsync(setting).ConfigureAwait(false);
            }
        }

        protected virtual async Task<SettingCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
            var cacheItem = await Cache.GetAsync(cacheKey).ConfigureAwait(false);

            if (cacheItem != null)
            {
                return cacheItem;
            }

            var setting = await SettingRepository.FindAsync(name, providerName, providerKey).ConfigureAwait(false);

            cacheItem = new SettingCacheItem(setting?.Value);

            await Cache.SetAsync(
                cacheKey,
                cacheItem
            ).ConfigureAwait(false);

            return cacheItem;
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
