using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpStringToEnumConverter<T> : JsonConverter<T>
        where T : struct, Enum
    {
        private readonly JsonStringEnumConverter _innerJsonStringEnumConverter;

        private JsonSerializerOptions _readJsonSerializerOptions;
        
        private JsonSerializerOptions _writeJsonSerializerOptions;
        
        public AbpStringToEnumConverter()
            : this(namingPolicy: null, allowIntegerValues: true)
        {

        }

        public AbpStringToEnumConverter(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
        {
            _innerJsonStringEnumConverter = new JsonStringEnumConverter(namingPolicy, allowIntegerValues);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x =>
                    x == this ||
                    x.GetType() == typeof(AbpStringToEnumFactory),
                _innerJsonStringEnumConverter.CreateConverter(typeToConvert, options));

            return JsonSerializer.Deserialize<T>(ref reader, _readJsonSerializerOptions);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x =>
                x == this ||
                x.GetType() == typeof(AbpStringToEnumFactory));

            JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
        }
    }
}
