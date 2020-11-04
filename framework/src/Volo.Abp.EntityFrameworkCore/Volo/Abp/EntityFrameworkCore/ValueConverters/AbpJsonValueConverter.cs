using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public class AbpJsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
    {
        public AbpJsonValueConverter()
            : base(
                d => SerializeObject(d),
                s => DeserializeObject(s))
        {

        }

        private static string SerializeObject(TPropertyType d)
        {
            return JsonSerializer.Serialize(d);
        }

        private static TPropertyType DeserializeObject(string s)
        {
            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new ObjectToInferredTypesConverter());
            return JsonSerializer.Deserialize<TPropertyType>(s, deserializeOptions);
        }
    }
}
