using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpDefaultJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public AbpDefaultJsonTypeInfoResolver(IOptions<AbpSystemTextJsonSerializerModifiersOptions> options)
    {
        foreach (var modifier in options.Value.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }
}
