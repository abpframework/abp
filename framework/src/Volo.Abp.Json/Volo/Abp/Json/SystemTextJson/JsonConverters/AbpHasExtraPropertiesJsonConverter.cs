using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpHasExtraPropertiesJsonConverter<T> : JsonConverter<T>
        where T : IHasExtraProperties
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, x =>
                x == this ||
                x.GetType() == typeof(AbpHasExtraPropertiesJsonConverterFactory));

            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var extensibleObject = JsonSerializer.Deserialize<T>(rootElement.GetRawText(), newOptions);

            var extraProperties = rootElement.EnumerateObject().FirstOrDefault(x =>
                    x.Name.Equals(nameof(IHasExtraProperties.ExtraProperties), StringComparison.OrdinalIgnoreCase))
                .Value.GetRawText();
            var extraPropertyDictionary = JsonSerializer.Deserialize(extraProperties, typeof(ExtraPropertyDictionary), newOptions);
            ObjectHelper.TrySetProperty(extensibleObject, x => x.ExtraProperties, () => extraPropertyDictionary);

            return extensibleObject;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, x =>
                x == this ||
                x.GetType() == typeof(AbpHasExtraPropertiesJsonConverterFactory));

            JsonSerializer.Serialize(writer, value, newOptions);
        }
    }
}
