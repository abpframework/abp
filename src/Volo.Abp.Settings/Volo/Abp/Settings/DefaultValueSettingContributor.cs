using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class DefaultValueSettingContributor : SettingContributor
    {
        public override string EntityType => null;

        public DefaultValueSettingContributor(ISettingStore settingStore) 
            : base(settingStore)
        {

        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            return Task.FromResult(setting.DefaultValue);
        }
    }
}