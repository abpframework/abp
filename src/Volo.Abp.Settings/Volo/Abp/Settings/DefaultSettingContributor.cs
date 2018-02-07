using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class DefaultSettingContributor : ISettingContributor, ISingletonDependency
    {
        private readonly ISettingStore _settingStore;

        public DefaultSettingContributor(ISettingStore settingStore)
        {
            _settingStore = settingStore;
        }
        
        public async Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true)
        {
            //TODO: Optimization: Get all settings and cache it!
            return await _settingStore.GetOrNullAsync(name, null, null);
        }
    }
}