using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class LocalizableStringInfoJsonConverter : JsonConverter<LocalizableStringInfo>
    {
        public override LocalizableStringInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            return new LocalizableStringInfo(
                rootElement.GetProperty("ResourceName").GetString(),
                rootElement.GetProperty("Name").GetString()
            );
        }

        public override void Write(Utf8JsonWriter writer, LocalizableStringInfo value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
