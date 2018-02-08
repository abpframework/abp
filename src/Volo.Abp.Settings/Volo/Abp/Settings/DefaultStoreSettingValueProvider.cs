using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultStoreSettingValueProvider : SettingValueProvider
    {
        public override string EntityType => null;

        public DefaultStoreSettingValueProvider(ISettingStore settingStore) 
            : base(settingStore)
        {
        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            return SettingStore.GetOrNullAsync(setting.Name, null, null);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string entityId)
        {
            return SettingStore.SetAsync(setting.Name, value, null, null);
        }

        public override Task ClearAsync(SettingDefinition setting, string entityId)
        {
            return SettingStore.DeleteAsync(setting.Name, null, null);
        }
    }
}