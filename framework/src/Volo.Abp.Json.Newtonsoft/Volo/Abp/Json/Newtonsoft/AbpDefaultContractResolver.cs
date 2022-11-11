using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpDefaultContractResolver : DefaultContractResolver, ITransientDependency
{
    private readonly Lazy<AbpDateTimeConverter> _dateTimeConverter;

    public AbpDefaultContractResolver(IServiceProvider serviceProvider)
    {
        _dateTimeConverter = new Lazy<AbpDateTimeConverter>(
            serviceProvider.GetRequiredService<AbpDateTimeConverter>,
            true
        );
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (AbpDateTimeConverter.ShouldNormalize(member, property))
        {
            property.Converter = _dateTimeConverter.Value;
        }

        return property;
    }
}
