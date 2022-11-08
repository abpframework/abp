using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpDefaultJsonTypeInfoResolver : DefaultJsonTypeInfoResolver, ITransientDependency
{
    public AbpDefaultJsonTypeInfoResolver(IOptions<AbpSystemTextJsonSerializerModifiersOptions> options)
    {
        foreach (var modifier in options.Value.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }
}
