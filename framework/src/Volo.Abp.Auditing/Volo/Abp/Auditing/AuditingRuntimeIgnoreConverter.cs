using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.Auditing
{
    public class AuditingRuntimeIgnoreConverter : JsonConverter<Dictionary<string, object>>
    {
        private readonly List<Type> _ignoredTypes;

        public AuditingRuntimeIgnoreConverter(List<Type> ignoredTypes)
        {
            _ignoredTypes = ignoredTypes;
        }

        public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
        {
            var newDictionary = new Dictionary<string, object>();
            foreach (var item in value.Where(x => x.Value != null))
            {
                if (item.GetType().IsDefined(typeof(DisableAuditingAttribute), true) ||
                    item.GetType().IsDefined(typeof(JsonIgnoreAttribute), true))
                {
                    continue;
                }

                if (_ignoredTypes.Any(x => x.IsInstanceOfType(item.Value)))
                {
                    continue;
                }

                newDictionary[item.Key] = item.Value;
            }

            JsonSerializer.Serialize(writer, newDictionary);
        }
    }
}
