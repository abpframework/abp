using Volo.Abp.Collections;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.Json;

public class AbpJsonOptions
{
    /// <summary>
    /// Used to set default value for the DateTimeFormat.
    /// </summary>
    public string DefaultDateTimeFormat { get; set; }

    /// <summary>
    /// It will try to use System.Json.Text to handle JSON if it can otherwise use Newtonsoft.
    /// Affects both AbpJsonModule and AbpAspNetCoreMvcModule.
    /// See <see cref="AbpSystemTextJsonUnsupportedTypeMatcher"/>
    /// </summary>
    public bool UseHybridSerializer { get; set; }

    public ITypeList<IJsonSerializerProvider> Providers { get; }

    public AbpJsonOptions()
    {
        Providers = new TypeList<IJsonSerializerProvider>();
        UseHybridSerializer = true;
    }
}
