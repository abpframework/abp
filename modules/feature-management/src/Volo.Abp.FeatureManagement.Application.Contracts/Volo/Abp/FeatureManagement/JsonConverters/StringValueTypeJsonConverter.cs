using System;
using System.Linq;
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

            var nameJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IStringValueType.Name), StringComparison.OrdinalIgnoreCase));
            if (nameJsonProperty.Value.ValueKind == JsonValueKind.String)
            {
                var name = nameJsonProperty.Value.GetString();

                var newOptions = JsonSerializerOptionsHelper.Create(options, this, new ValueValidatorJsonConverter(),
                    new SelectionStringValueItemSourceJsonConverter());

                return name switch
                {
                    "SelectionStringValueType" =>  JsonSerializer.Deserialize<SelectionStringValueType>(rootElement.GetRawText(), newOptions),
                    "FreeTextStringValueType" => JsonSerializer.Deserialize<FreeTextStringValueType>(rootElement.GetRawText(), newOptions),
                    "ToggleStringValueType" => JsonSerializer.Deserialize<ToggleStringValueType>(rootElement.GetRawText(), newOptions),
                    _ => throw new ArgumentException($"{nameof(IStringValueType)} named {name} was not found!")
                };
            }

            throw new JsonException($"Can't to get the {nameof(IStringValueType.Name)} property of {nameof(IStringValueType)}!");
        }

        public override void Write(Utf8JsonWriter writer, IStringValueType value, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, this);
            JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
        }
    }
}
