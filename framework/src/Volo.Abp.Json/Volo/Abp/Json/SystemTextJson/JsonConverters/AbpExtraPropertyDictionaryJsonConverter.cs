using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpExtraPropertyDictionaryJsonConverter<T> : JsonConverter<T>
        where T : ExtensibleObject
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.RemoveAll(x => x == this || x.GetType() == typeof(AbpExtraPropertyDictionaryJsonConverterFactory));

            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var extensibleObject = JsonSerializer.Deserialize<T>(rootElement.GetRawText(), newOptions);
            var extraProperties = rootElement.EnumerateObject().FirstOrDefault(x =>
                    x.Name.Equals(nameof(ExtensibleObject.ExtraProperties), StringComparison.OrdinalIgnoreCase))
                .Value.GetRawText();

            var extraPropertyDictionary = JsonSerializer.Deserialize(extraProperties, typeof(ExtraPropertyDictionary), newOptions);

            ObjectHelper.TrySetProperty(extensibleObject, x => x.ExtraProperties, () => extraPropertyDictionary);

            return extensibleObject;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.RemoveAll(x => x == this || x.GetType() == typeof(AbpExtraPropertyDictionaryJsonConverterFactory));
            JsonSerializer.Serialize(writer, value, newOptions);
        }
    }
}
