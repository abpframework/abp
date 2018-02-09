using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingStore : AbpServiceBase, ISettingStore, ITransientDependency
    {
        private readonly ISettingRepository _settingRepository;

        public SettingStore(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var setting = await _settingRepository.FindAsync(name, providerName, providerKey);
            return setting?.Value;
        }

        public async Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            var setting = await _settingRepository.FindAsync(name, providerName, providerKey);
            if (setting == null)
            {
                setting = new Setting(GuidGenerator.Create(), name, value, providerName, providerKey);
                await _settingRepository.InsertAsync(setting);
            }

            setting.Value = value;
            await _settingRepository.UpdateAsync(setting);
        }

        public async Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
        {
            var setting = await _settingRepository.GetListAsync(providerName, providerKey);
            return setting.Select(s => new SettingValue(s.Name, s.Value)).ToList();
        }

        public async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var setting = await _settingRepository.FindAsync(name, providerName, providerKey);
            if (setting != null)
            {
                await _settingRepository.DeleteAsync(setting);
            }
        }
    }
}
