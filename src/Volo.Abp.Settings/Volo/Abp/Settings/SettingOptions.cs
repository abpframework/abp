using Volo.Abp.Collections;

namespace Volo.Abp.Settings
{
    public class SettingOptions
    {
        public ITypeList<ISettingContributor> Contributors { get; }

        public SettingOptions()
        {
            Contributors = new TypeList<ISettingContributor>
            {
                typeof(DefaultSettingContributor)
            };
        }
    }
}
