using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public class GlobalSettingManagementProvider : SettingManagementProvider, ITransientDependency
    {
        public override string Name => GlobalSettingValueProvider.ProviderName;

        public GlobalSettingManagementProvider(ISettingManagementStore settingManagementStore) 
            : base(settingManagementStore)
        {

        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            return null;
        }
    }
}