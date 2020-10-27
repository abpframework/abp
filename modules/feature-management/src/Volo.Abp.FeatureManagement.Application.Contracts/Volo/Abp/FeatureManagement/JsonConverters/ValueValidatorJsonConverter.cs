using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Json.Microsoft.JsonConverters;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    public class ValueValidatorJsonConverter : JsonConverter<IValueValidator>
    {
        public override IValueValidator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var valueValidator = CreateValueValidatorByName(rootElement.GetProperty("Name").GetString());

            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new ObjectToInferredTypesConverter());
            var properties = JsonSerializer.Deserialize<IDictionary<string, object>>(rootElement.GetProperty("Properties").GetRawText(), deserializeOptions);
            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    valueValidator.Properties[property.Key] = property.Value;
                }
            }

            return valueValidator;
        }

        public override void Write(Utf8JsonWriter writer, IValueValidator value, JsonSerializerOptions options)
        {
            if (value.GetType() == typeof(AlwaysValidValueValidator))
            {
                JsonSerializer.Serialize(writer, (AlwaysValidValueValidator)value);
            }
            else if (value.GetType() == typeof(BooleanValueValidator))
            {
                JsonSerializer.Serialize(writer, (BooleanValueValidator)value);
            }
            else if (value.GetType() == typeof(NumericValueValidator))
            {
                JsonSerializer.Serialize(writer, (NumericValueValidator)value);
            }
            else if (value.GetType() == typeof(StringValueValidator))
            {
                JsonSerializer.Serialize(writer, (StringValueValidator)value);
            }
            else
            {
                throw new JsonException("Unknown IValueValidator type!");
            }
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
