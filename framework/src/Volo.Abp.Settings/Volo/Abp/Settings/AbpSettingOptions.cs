using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Settings;

public class AbpSettingOptions
{
    public ITypeList<ISettingDefinitionProvider> DefinitionProviders { get; }

    public ITypeList<ISettingValueProvider> ValueProviders { get; }

    public HashSet<string> DeletedSettings { get; }

    public AbpSettingOptions()
    {
        DefinitionProviders = new TypeList<ISettingDefinitionProvider>();
        ValueProviders = new TypeList<ISettingValueProvider>();
        DeletedSettings = new HashSet<string>();
    }
}
