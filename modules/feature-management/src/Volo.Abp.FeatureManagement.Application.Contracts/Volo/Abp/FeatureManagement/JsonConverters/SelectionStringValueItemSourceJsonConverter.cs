using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters;

public class SelectionStringValueItemSourceJsonConverter : JsonConverter<ISelectionStringValueItemSource>
{
    private JsonSerializerOptions _readJsonSerializerOptions;

    private JsonSerializerOptions _writeJsonSerializerOptions;

    public override ISelectionStringValueItemSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
        var itemsJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x =>
            x.Name.Equals(nameof(ISelectionStringValueItemSource.Items), StringComparison.OrdinalIgnoreCase));

        if (itemsJsonProperty.Value.ValueKind == JsonValueKind.Array)
        {
            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);

            var selectionStringValueItem =
                itemsJsonProperty.Value.Deserialize<LocalizableSelectionStringValueItem[]>(_readJsonSerializerOptions) ??
                Array.Empty<LocalizableSelectionStringValueItem>();

            return new StaticSelectionStringValueItemSource(selectionStringValueItem.As<ISelectionStringValueItem[]>());
        }

        throw new JsonException($"Can't to get the {nameof(ISelectionStringValueItemSource.Items)} property of {nameof(ISelectionStringValueItemSource)}!");
    }

    public override void Write(Utf8JsonWriter writer, ISelectionStringValueItemSource value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        JsonSerializer.Serialize(writer, value, value.GetType(), _writeJsonSerializerOptions);
    }
}
