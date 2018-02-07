using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultStoreSettingContributor : SettingContributor
    {
        public override string EntityType => null;

        public DefaultStoreSettingContributor(ISettingStore settingStore) 
            : base(settingStore)
        {
        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            return SettingStore.GetOrNullAsync(setting.Name, null, null);
        }
    }
}