using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Features;

public class AbpFeatureOptions
{
    public ITypeList<IFeatureDefinitionProvider> DefinitionProviders { get; }

    public ITypeList<IFeatureValueProvider> ValueProviders { get; }

    public HashSet<string> DeletedFeatures { get; }

    public HashSet<string> DeletedFeatureGroups { get; }

    public AbpFeatureOptions()
    {
        DefinitionProviders = new TypeList<IFeatureDefinitionProvider>();
        ValueProviders = new TypeList<IFeatureValueProvider>();

        DeletedFeatures = new HashSet<string>();
        DeletedFeatureGroups = new HashSet<string>();
    }
}
