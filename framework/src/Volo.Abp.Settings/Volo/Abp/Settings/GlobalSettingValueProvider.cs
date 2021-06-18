using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class GlobalSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "G";

        public override string Name => ProviderName;

        public GlobalSettingValueProvider(ISettingStore settingStore)
            : base(settingStore)
        {
        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            return SettingStore.GetOrNullAsync(setting.Name, Name, null);
        }

        public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return SettingStore.GetAllAsync(settings.Select(x => x.Name).ToArray(), Name, null);
        }
    }
}
