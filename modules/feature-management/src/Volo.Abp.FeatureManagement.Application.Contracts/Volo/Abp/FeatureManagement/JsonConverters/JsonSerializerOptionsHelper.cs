using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.FeatureManagement.JsonConverters
{
    internal static class JsonSerializerOptionsHelper
    {
        public static JsonSerializerOptions Create(JsonSerializerOptions baseOptions, JsonConverter removeConverter, params JsonConverter[] addConverters)
        {
            var options = new JsonSerializerOptions(baseOptions);
            options.Converters.RemoveAll(x => x == removeConverter);
            options.Converters.AddIfNotContains(addConverters);
            return options;
        }
    }
}
