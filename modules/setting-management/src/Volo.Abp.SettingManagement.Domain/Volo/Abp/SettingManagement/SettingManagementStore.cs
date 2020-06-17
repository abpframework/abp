using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementStore : ISettingManagementStore, ITransientDependency
    {
        protected SettingManagementStoreCache Cache { get; }
        protected ISettingRepository SettingRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public SettingManagementStore(
            ISettingRepository settingRepository,
            IGuidGenerator guidGenerator,
            SettingManagementStoreCache cache)
        {
            SettingRepository = settingRepository;
            GuidGenerator = guidGenerator;
            Cache = cache;
        }

        [UnitOfWork]
        public virtual async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var cacheItem = await Cache.GetOrAddAsync(CalculateCacheKey(name, providerName, providerKey),
                async () => new SettingCacheItem((await SettingRepository.FindAsync(name, providerName, providerKey))?.Value));
            return cacheItem.Value;
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

            await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new SettingCacheItem(setting.Value));
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
                await Cache.RemoveAsync(CalculateCacheKey(name, providerName, providerKey));
            }
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
