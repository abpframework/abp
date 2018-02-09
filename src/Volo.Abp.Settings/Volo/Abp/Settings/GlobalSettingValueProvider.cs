using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class GlobalSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "Global";

        public override string Name => ProviderName;

        public GlobalSettingValueProvider(ISettingStore settingStore) 
            : base(settingStore)
        {
        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            return SettingStore.GetOrNullAsync(setting.Name, Name, null);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            return SettingStore.SetAsync(setting.Name, value, Name, null);
        }

        public override Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            return SettingStore.DeleteAsync(setting.Name, Name, null);
        }
    }
}