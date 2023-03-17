using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpDefaultContractResolver : DefaultContractResolver, ITransientDependency
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
