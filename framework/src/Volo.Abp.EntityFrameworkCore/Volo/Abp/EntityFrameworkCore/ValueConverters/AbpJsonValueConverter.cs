using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters;

public class AbpJsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
{
    public AbpJsonValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s))
    {

    }

    public readonly static JsonSerializerOptions SerializeOptions = new JsonSerializerOptions();

    private static string SerializeObject(TPropertyType d)
    {
        return JsonSerializer.Serialize(d, SerializeOptions);
    }

    public readonly static JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };

    private static TPropertyType DeserializeObject(string s)
    {
        return JsonSerializer.Deserialize<TPropertyType>(s, DeserializeOptions)!;
    }
}
