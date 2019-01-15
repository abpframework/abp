using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultValueSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "Default";

        public override string Name => ProviderName;

        public DefaultValueSettingValueProvider(ISettingStore settingStore) 
            : base(settingStore)
        {

        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            return Task.FromResult(setting.DefaultValue);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            setting.DefaultValue = value;
            return Task.CompletedTask;
        }

        public override Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            setting.DefaultValue = null;
            return Task.CompletedTask;
        }
    }
}