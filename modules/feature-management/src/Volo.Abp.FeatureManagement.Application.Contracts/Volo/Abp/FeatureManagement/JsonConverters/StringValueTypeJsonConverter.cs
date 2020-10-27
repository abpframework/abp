using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class StringValueTypeJsonConverter : JsonConverter<IStringValueType>
    {
        public override IStringValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new ValueValidatorJsonConverter());
            jsonSerializerOptions.Converters.Add(new SelectionStringValueItemSourceJsonConverter());

            var name = rootElement.GetProperty("Name").GetString();
            return name switch
            {
                "SelectionStringValueType" =>  JsonSerializer.Deserialize<SelectionStringValueType>(rootElement.GetRawText(), jsonSerializerOptions),
                "FreeTextStringValueType" => JsonSerializer.Deserialize<FreeTextStringValueType>(rootElement.GetRawText(), jsonSerializerOptions),
                "ToggleStringValueType" => JsonSerializer.Deserialize<ToggleStringValueType>(rootElement.GetRawText(), jsonSerializerOptions),
                _ => throw new ArgumentException($"{nameof(IStringValueType)} named {name} was not found!")
            };
        }

        public override void Write(Utf8JsonWriter writer, IStringValueType value, JsonSerializerOptions options)
        {
            if (value.GetType() == typeof(FreeTextStringValueType))
            {
                JsonSerializer.Serialize(writer, (FreeTextStringValueType)value);
            }
            else if (value.GetType() == typeof(SelectionStringValueType))
            {
                JsonSerializer.Serialize(writer, (SelectionStringValueType)value);
            }
            else if (value.GetType() == typeof(ToggleStringValueType))
            {
                JsonSerializer.Serialize(writer, (ToggleStringValueType)value);
            }
            else
            {
                throw new JsonException("Unknown IStringValueType type!");
            }
        }
    }
}
