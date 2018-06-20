using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public class SettingStore : ISettingStore, ITransientDependency
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IGuidGenerator _guidGenerator;

        public SettingStore(ISettingRepository settingRepository, IGuidGenerator guidGenerator)
        {
            _settingRepository = settingRepository;
            _guidGenerator = guidGenerator;
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
                setting = new Setting(_guidGenerator.Create(), name, value, providerName, providerKey);
                await _settingRepository.InsertAsync(setting);
            }
            else
            {
                setting.Value = value;
                await _settingRepository.UpdateAsync(setting);
            }
        }

        public async Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
        {
            var settings = await _settingRepository.GetListAsync(providerName, providerKey);
            return settings.Select(s => new SettingValue(s.Name, s.Value)).ToList();
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
