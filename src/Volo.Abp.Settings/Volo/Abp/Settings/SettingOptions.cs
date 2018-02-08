using Volo.Abp.Collections;

namespace Volo.Abp.Settings
{
    public class SettingOptions
    {
        public ITypeList<ISettingDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<ISettingValueProvider> ValueProviders { get; }

        public SettingOptions()
        {
            DefinitionProviders = new TypeList<ISettingDefinitionProvider>();
            ValueProviders = new TypeList<ISettingValueProvider>
            {
                typeof(DefaultValueSettingValueProvider),
                typeof(GlobalSettingValueProvider)
            };
        }
    }
}
