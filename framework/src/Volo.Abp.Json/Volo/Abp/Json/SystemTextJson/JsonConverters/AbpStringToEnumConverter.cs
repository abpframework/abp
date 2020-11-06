using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpStringToEnumConverter : JsonConverter<object>
    {
        private readonly JsonStringEnumConverter _innerJsonStringEnumConverter;

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

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Remove(this);
            newOptions.Converters.Add(_innerJsonStringEnumConverter.CreateConverter(typeToConvert, options));
            return JsonSerializer.Deserialize(ref reader, typeToConvert, newOptions);
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Remove(this);
            JsonSerializer.Serialize(writer, value, newOptions);
        }
    }
}
