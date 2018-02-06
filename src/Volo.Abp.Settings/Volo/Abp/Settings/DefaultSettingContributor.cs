using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultSettingContributor : ISettingContributor
    {
        private readonly ISettingStore _settingStore;

        public DefaultSettingContributor(ISettingStore settingStore)
        {
            _settingStore = settingStore;
        }

        public async Task<string> GetOrNull(string name)
        {
            //Optimization: Get all settings and cache it!
            return await _settingStore.GetOrNullAsync(name, null, null);
        }
    }
}