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

        public override Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            return Task.FromResult(setting.DefaultValue);
        }
    }
}