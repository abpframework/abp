﻿using System;
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
        public override IValueValidator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            var name = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals("Name", StringComparison.InvariantCultureIgnoreCase)).Value.GetString();
            var valueValidator = CreateValueValidatorByName(name);

            var propertiesRawText = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals("Properties", StringComparison.InvariantCultureIgnoreCase)).Value.GetRawText();
            var newOptions = JsonSerializerOptionsHelper.Create(options, this, new ObjectToInferredTypesConverter());
            var properties = JsonSerializer.Deserialize<IDictionary<string, object>>(propertiesRawText, newOptions);
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
            var newOptions = JsonSerializerOptionsHelper.Create(options, this);
            JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
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
