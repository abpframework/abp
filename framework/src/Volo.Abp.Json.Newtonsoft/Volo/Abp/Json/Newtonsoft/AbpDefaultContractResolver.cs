using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpDefaultContractResolver : DefaultContractResolver
{
    private readonly AbpDateTimeConverter _dateTimeConverter;

    public AbpDefaultContractResolver(AbpDateTimeConverter dateTimeConverter)
    {
        _dateTimeConverter = dateTimeConverter;
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (AbpDateTimeConverter.ShouldNormalize(member, property))
        {
            property.Converter = _dateTimeConverter;
        }

        return property;
    }
}
