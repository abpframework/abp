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

        public async Task<string> GetOrNullAsync(string name, string entityType, string entityId)
        {
            var setting = await _settingRepository.FindAsync(name, entityType, entityId);
            return setting?.Value;
        }

        public async Task SetAsync(string name, string value, string entityType, string entityId)
        {
            var setting = await _settingRepository.FindAsync(name, entityType, entityId);
            if (setting == null)
            {
                setting = new Setting(GuidGenerator.Create(), name, value, entityType, entityId, CurrentTenant.Id);
                await _settingRepository.InsertAsync(setting);
            }

            setting.Value = value;
            await _settingRepository.UpdateAsync(setting);
        }

        public async Task<List<SettingValue>> GetListAsync(string entityType, string entityId)
        {
            var setting = await _settingRepository.GetListAsync(entityType, entityId);
            return setting.Select(s => new SettingValue(s.Name, s.Value)).ToList();
        }
    }
}
