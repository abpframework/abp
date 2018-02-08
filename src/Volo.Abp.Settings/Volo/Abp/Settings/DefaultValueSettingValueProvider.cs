using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultValueSettingValueProvider : SettingValueProvider
    {
        public override string EntityType => null;

        public DefaultValueSettingValueProvider(ISettingStore settingStore) 
            : base(settingStore)
        {

        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            return Task.FromResult(setting.DefaultValue);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string entityId)
        {
            setting.DefaultValue = value;
            return Task.CompletedTask;
        }

        public override Task ClearAsync(SettingDefinition setting, string entityId)
        {
            setting.DefaultValue = null;
            return Task.CompletedTask;
        }
    }
}