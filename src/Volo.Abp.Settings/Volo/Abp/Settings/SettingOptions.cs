using Volo.Abp.Collections;

namespace Volo.Abp.Settings
{
    public class SettingOptions
    {
        public ITypeList<ISettingContributor> Contributors { get; }

        public ITypeList<ISettingProvider> Providers { get; set; }

        public SettingOptions()
        {
            Contributors = new TypeList<ISettingContributor>
            {
                typeof(DefaultSettingContributor)
            };

            Providers = new TypeList<ISettingProvider>();
        }
    }
}
