using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public class AbpHasExtraPropertiesJsonConverter<T> : JsonConverter<T>
    where T : IHasExtraProperties
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var newOptions = JsonSerializerOptionsHelper.Create(options, x => x == this);

        var converterFactory = newOptions.Converters.FirstOrDefault(x => x is AbpHasExtraPropertiesJsonConverterFactory).As<AbpHasExtraPropertiesJsonConverterFactory>();
        var newConverterFactory = new AbpHasExtraPropertiesJsonConverterFactory();
        if (converterFactory != null)
        {
            newOptions.Converters.Remove(converterFactory);
            newConverterFactory.AddExcludeTypes(converterFactory.GetExcludeTypes().ToArray());
        }

        newConverterFactory.AddExcludeTypes(typeToConvert);
        newOptions.Converters.Add(newConverterFactory);

        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
        if (rootElement.ValueKind == JsonValueKind.Object)
        {
            var extensibleObject = rootElement.Deserialize<T>(newOptions);

            var extraPropertiesJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IHasExtraProperties.ExtraProperties), StringComparison.OrdinalIgnoreCase));
            
            if (extraPropertiesJsonProperty.Value.ValueKind == JsonValueKind.Object)
            {
                var extraPropertyDictionary = extraPropertiesJsonProperty.Value.Deserialize(typeof(ExtraPropertyDictionary), newOptions);
                ObjectHelper.TrySetProperty(extensibleObject, x => x.ExtraProperties, () => extraPropertyDictionary);
            }

            return extensibleObject;
        }

        throw new JsonException("RootElement's ValueKind is not Object!");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var newOptions = JsonSerializerOptionsHelper.Create(options, x =>
            x == this ||
            x.GetType() == typeof(AbpHasExtraPropertiesJsonConverterFactory));

        JsonSerializer.Serialize(writer, value, newOptions);
    }
}
