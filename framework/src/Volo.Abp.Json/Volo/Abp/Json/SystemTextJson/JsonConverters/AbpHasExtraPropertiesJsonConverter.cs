using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpHasExtraPropertiesJsonConverter<T> : JsonConverter<T>
        where T : IHasExtraProperties
    {
        private JsonSerializerOptions _readJsonSerializerOptions;
        
        private JsonSerializerOptions _writeJsonSerializerOptions;
        
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x => x == this);
            
            var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
            if (rootElement.ValueKind == JsonValueKind.Object)
            {
                var converterFactory = _readJsonSerializerOptions.Converters
                    .FirstOrDefault(x => x is AbpHasExtraPropertiesJsonConverterFactory)
                    .As<AbpHasExtraPropertiesJsonConverterFactory>();

                T extensibleObject;
                using (converterFactory != null ? converterFactory.Exclude(typeToConvert) : NullDisposable.Instance)
                {
                    extensibleObject = rootElement.Deserialize<T>(_readJsonSerializerOptions);
                }

                var extraPropertiesJsonProperty = rootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals(nameof(IHasExtraProperties.ExtraProperties), StringComparison.OrdinalIgnoreCase));
                
                if (extraPropertiesJsonProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    var extraPropertyDictionary = extraPropertiesJsonProperty.Value.Deserialize(typeof(ExtraPropertyDictionary), _readJsonSerializerOptions);
                    ObjectHelper.TrySetProperty(extensibleObject, x => x.ExtraProperties, () => extraPropertyDictionary);
                }

                return extensibleObject;
            }

            throw new JsonException("RootElement's ValueKind is not Object!");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x =>
                x == this ||
                x.GetType() == typeof(AbpHasExtraPropertiesJsonConverterFactory));

            JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
        }
    }
}
