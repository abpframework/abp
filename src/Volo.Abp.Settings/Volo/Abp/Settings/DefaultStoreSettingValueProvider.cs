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
    }
}