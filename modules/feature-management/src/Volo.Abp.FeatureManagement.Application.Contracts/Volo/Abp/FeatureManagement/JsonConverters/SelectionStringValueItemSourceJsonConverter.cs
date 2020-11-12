using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class SelectionStringValueItemSourceJsonConverter : JsonConverter<ISelectionStringValueItemSource>
    {
        public override ISelectionStringValueItemSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, this);

            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var items = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(ISelectionStringValueItemSource.Items), StringComparison.OrdinalIgnoreCase)).Value.GetRawText();

            var selectionStringValueItem =
                JsonSerializer.Deserialize<List<LocalizableSelectionStringValueItem>>(items, newOptions) ??
                new List<LocalizableSelectionStringValueItem>();

            return new StaticSelectionStringValueItemSource(selectionStringValueItem.ToArray().As<ISelectionStringValueItem[]>());
        }

        public override void Write(Utf8JsonWriter writer, ISelectionStringValueItemSource value, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, this);
            JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
        }
    }
}
