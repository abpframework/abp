using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class SelectionStringValueItemSourceJsonConverter : JsonConverter<ISelectionStringValueItemSource>
    {
        public override ISelectionStringValueItemSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new SelectionStringValueItemJsonConverter());

            var selectionStringValueItem =
                JsonSerializer.Deserialize<List<LocalizableSelectionStringValueItem>>(rootElement.GetProperty("Items").GetRawText(), jsonSerializerOptions) ??
                new List<LocalizableSelectionStringValueItem>();

            return new StaticSelectionStringValueItemSource(selectionStringValueItem.ToArray());
        }

        public override void Write(Utf8JsonWriter writer, ISelectionStringValueItemSource value, JsonSerializerOptions options)
        {
            if (value.GetType() == typeof(StaticSelectionStringValueItemSource))
            {
                JsonSerializer.Serialize(writer, (StaticSelectionStringValueItemSource)value);
            }
            else
            {
                throw new JsonException("Unknown ISelectionStringValueItemSource type!");
            }
        }
    }
}
