using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class ValueValidatorJsonConverter : JsonConverter<IValueValidator>
    {
        private JsonSerializerOptions _readJsonSerializerOptions;
        
        private JsonSerializerOptions _writeJsonSerializerOptions;
        
        public override IValueValidator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;

            var nameJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IValueValidator.Name), StringComparison.OrdinalIgnoreCase));
            if (nameJsonProperty.Value.ValueKind == JsonValueKind.String)
            {
                var valueValidator = CreateValueValidatorByName(nameJsonProperty.Value.GetString());

                var propertiesJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IValueValidator.Properties), StringComparison.OrdinalIgnoreCase));
                if (propertiesJsonProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this, new ObjectToInferredTypesConverter());
                    var properties = propertiesJsonProperty.Value.Deserialize<IDictionary<string, object>>(_readJsonSerializerOptions);
                    if (properties != null && properties.Any())
                    {
                        foreach (var property in properties)
                        {
                            valueValidator.Properties[property.Key] = property.Value;
                        }
                    }
                }

                return valueValidator;
            }

            throw new JsonException($"Can't to get the {nameof(IValueValidator.Name)} property of {nameof(IValueValidator)}!");
        }

        public override void Write(Utf8JsonWriter writer, IValueValidator value, JsonSerializerOptions options)
        {
            _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
            JsonSerializer.Serialize(writer, value, value.GetType(), _writeJsonSerializerOptions);
        }

        protected virtual IValueValidator CreateValueValidatorByName(string name)
        {
            return name switch
            {
                "NULL" => new AlwaysValidValueValidator(),
                "BOOLEAN" => new BooleanValueValidator(),
                "NUMERIC" => new NumericValueValidator(),
                "STRING" => new StringValueValidator(),
                _ => throw new ArgumentException($"{nameof(IValueValidator)} named {name} was not found!")
            };
        }
    }
}
