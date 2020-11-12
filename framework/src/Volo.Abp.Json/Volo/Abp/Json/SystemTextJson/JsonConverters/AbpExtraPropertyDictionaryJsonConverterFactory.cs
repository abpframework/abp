using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpExtraPropertyDictionaryJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ExtensibleObject) || typeToConvert.IsSubclassOf(typeof(ExtensibleObject));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter) Activator.CreateInstance(
                typeof(AbpExtraPropertyDictionaryJsonConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                null,
                culture: null)!;
        }
    }
}
