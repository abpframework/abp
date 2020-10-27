using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class SelectionStringValueItemJsonConverter : JsonConverter<ISelectionStringValueItem>
    {
        public override ISelectionStringValueItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;

            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new LocalizableStringInfoJsonConverter());

            return new LocalizableSelectionStringValueItem
            {
                Value = rootElement.GetProperty("Value").GetString(),
                DisplayText = JsonSerializer.Deserialize<LocalizableStringInfo>(rootElement.GetProperty("DisplayText").GetRawText(), jsonSerializerOptions)
            };
        }

        public override void Write(Utf8JsonWriter writer, ISelectionStringValueItem value, JsonSerializerOptions options)
        {
            if (value.GetType() == typeof(LocalizableStringInfo))
            {
                JsonSerializer.Serialize(writer, (LocalizableSelectionStringValueItem)value);
            }
            else
            {
                throw new JsonException("Unknown ISelectionStringValueItem type!");
            }
        }
    }
}
