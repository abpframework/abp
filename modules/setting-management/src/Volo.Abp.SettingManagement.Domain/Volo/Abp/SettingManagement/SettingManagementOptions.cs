using Volo.Abp.Collections;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementOptions
    {
        public ITypeList<ISettingManagementProvider> Providers { get; }

        public SettingManagementOptions()
        {
            Providers = new TypeList<ISettingManagementProvider>();
        }
    }
}
