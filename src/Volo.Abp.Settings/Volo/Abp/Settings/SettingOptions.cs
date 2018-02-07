using Volo.Abp.Collections;

namespace Volo.Abp.Settings
{
    public class SettingOptions
    {
        public ITypeList<ISettingProvider> Providers { get; set; }

        public ITypeList<ISettingContributor> Contributors { get; }

        public SettingOptions()
        {
            Providers = new TypeList<ISettingProvider>();
            Contributors = new TypeList<ISettingContributor>
            {
                typeof(DefaultValueSettingContributor),
                typeof(DefaultStoreSettingContributor)
            };
        }
    }
}
