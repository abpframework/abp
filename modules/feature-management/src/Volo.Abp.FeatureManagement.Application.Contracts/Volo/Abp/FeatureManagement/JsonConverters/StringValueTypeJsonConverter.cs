using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters;

public class StringValueTypeJsonConverter : JsonConverter<IStringValueType>
{
    private JsonSerializerOptions _readJsonSerializerOptions;

    private JsonSerializerOptions _writeJsonSerializerOptions;

    protected readonly AbpFeatureManagementApplicationContractsOptions Options;

    public StringValueTypeJsonConverter(AbpFeatureManagementApplicationContractsOptions options)
    {
        Options = options;
    }

    public override IStringValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;

        var nameJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IStringValueType.Name), StringComparison.OrdinalIgnoreCase));
        if (nameJsonProperty.Value.ValueKind == JsonValueKind.String)
        {
            var name = nameJsonProperty.Value.GetString();

            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this, new ValueValidatorJsonConverter(), new SelectionStringValueItemSourceJsonConverter());

            return name switch
            {
                "SelectionStringValueType" =>  rootElement.Deserialize<SelectionStringValueType>(_readJsonSerializerOptions),
                "FreeTextStringValueType" => rootElement.Deserialize<FreeTextStringValueType>(_readJsonSerializerOptions),
                "ToggleStringValueType" => rootElement.Deserialize<ToggleStringValueType>(_readJsonSerializerOptions),
                _ => throw new ArgumentException($"{nameof(IStringValueType)} named {name} was not found!")
            };
        }

        throw new JsonException($"Can't to get the {nameof(IStringValueType.Name)} property of {nameof(IStringValueType)}!");
    }

    public override void Write(Utf8JsonWriter writer, IStringValueType value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        JsonSerializer.Serialize(writer, value, value.GetType(), _writeJsonSerializerOptions);
    }
}
