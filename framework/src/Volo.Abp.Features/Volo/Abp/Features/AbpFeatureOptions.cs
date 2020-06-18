using Volo.Abp.Collections;

namespace Volo.Abp.Features
{
    public class AbpFeatureOptions
    {
        public ITypeList<IFeatureDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IFeatureValueProvider> ValueProviders { get; }

        public AbpFeatureOptions()
        {
            DefinitionProviders = new TypeList<IFeatureDefinitionProvider>();
            ValueProviders = new TypeList<IFeatureValueProvider>();
        }
    }
}
