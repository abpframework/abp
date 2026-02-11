using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Settings;

public class AbpSettingOptions
{
    public ITypeList<ISettingDefinitionProvider> DefinitionProviders { get; }

    public ITypeList<ISettingValueProvider> ValueProviders { get; }

    public HashSet<string> DeletedSettings { get; }

    /// <summary>
    /// Default: true.
    /// This is useful when you change <see cref="SettingDefinition.IsEncrypted"/> of an existing setting definition to true and don't want to lose the original value.
    /// </summary>
    public bool ReturnOriginalValueIfDecryptFailed { get; set; }

    public AbpSettingOptions()
    {
        DefinitionProviders = new TypeList<ISettingDefinitionProvider>();
        ValueProviders = new TypeList<ISettingValueProvider>();
        DeletedSettings = new HashSet<string>();
        ReturnOriginalValueIfDecryptFailed = true;
    }
}
