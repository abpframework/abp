using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing;

public class JsonAuditSerializer : IAuditSerializer, ITransientDependency
{
    protected AbpAuditingOptions Options;

    public JsonAuditSerializer(IOptions<AbpAuditingOptions> options)
    {
        Options = options.Value;
    }

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, CreateJsonSerializerOptions());
    }

    private static readonly ConcurrentDictionary<string, JsonSerializerOptions> JsonSerializerOptionsCache =
        new ConcurrentDictionary<string, JsonSerializerOptions>();

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return JsonSerializerOptionsCache.GetOrAdd(nameof(JsonAuditSerializer), _ =>
        {
            var settings = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers =
                    {
                        jsonTypeInfo =>
                        {
                            if (Options.IgnoredTypes.Any(ignoredType => ignoredType.IsAssignableFrom(jsonTypeInfo.Type)) ||
                                jsonTypeInfo.Type.GetCustomAttributes(typeof(DisableAuditingAttribute), false).Any())
                            {
                                if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
                                {
                                    jsonTypeInfo.Properties.Clear();
                                }
                            }

                            foreach (var property in jsonTypeInfo.Properties)
                            {
                                if (Options.IgnoredTypes.Any(ignoredType => ignoredType.IsAssignableFrom(property.PropertyType)))
                                {
                                    property.ShouldSerialize = (_, _) => false;
                                }

                                if (property.AttributeProvider != null &&
                                    property.AttributeProvider.GetCustomAttributes(typeof(DisableAuditingAttribute), false).Any())
                                {
                                    property.ShouldSerialize = (_, _) => false;
                                }

                                if (property.PropertyType.DeclaringType != null &&
                                    property.PropertyType.DeclaringType.IsDefined(typeof(DisableAuditingAttribute)))
                                {
                                    property.ShouldSerialize = (_, _) => false;
                                }
                            }
                        }
                    }
                }
            };

            return settings;
        });
    }
}
