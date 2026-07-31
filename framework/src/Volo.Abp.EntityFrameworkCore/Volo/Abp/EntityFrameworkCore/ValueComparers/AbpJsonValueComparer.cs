using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace Volo.Abp.EntityFrameworkCore.ValueComparers;

public class AbpJsonValueComparer<TPropertyType> : ValueComparer<TPropertyType>
{
    public AbpJsonValueComparer()
        : base(
            (left, right) => Serialize(left) == Serialize(right),
            v => Serialize(v).GetHashCode(),
            v => Deserialize(Serialize(v)))
    {
    }

    private static string Serialize(TPropertyType? value)
    {
        return JsonSerializer.Serialize(value, AbpJsonValueConverter<TPropertyType>.SerializeOptions);
    }

    private static TPropertyType Deserialize(string value)
    {
        return JsonSerializer.Deserialize<TPropertyType>(value, AbpJsonValueConverter<TPropertyType>.DeserializeOptions)!;
    }
}
