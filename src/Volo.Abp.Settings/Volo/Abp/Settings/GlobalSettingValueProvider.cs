using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class GlobalSettingValueProvider : SettingValueProvider
    {
        public const string DefaultEntityType = "Global";

        public override string EntityType => DefaultEntityType;

        public GlobalSettingValueProvider(ISettingStore settingStore) 
            : base(settingStore)
        {
        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            return SettingStore.GetOrNullAsync(setting.Name, EntityType, null);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string entityId)
        {
            return SettingStore.SetAsync(setting.Name, value, EntityType, null);
        }

        public override Task ClearAsync(SettingDefinition setting, string entityId)
        {
            return SettingStore.DeleteAsync(setting.Name, EntityType, null);
        }
    }
}