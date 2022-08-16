using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver, ITransientDependency
{
    private readonly Lazy<AbpJsonIsoDateTimeConverter> _dateTimeConverter;

    public AbpCamelCasePropertyNamesContractResolver(IServiceProvider serviceProvider)
    {
        _dateTimeConverter = new Lazy<AbpJsonIsoDateTimeConverter>(
            serviceProvider.GetRequiredService<AbpJsonIsoDateTimeConverter>,
            true
        );

        NamingStrategy = new CamelCaseNamingStrategy();
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        ModifyProperty(member, property);

        return property;
    }

    protected virtual void ModifyProperty(MemberInfo member, JsonProperty property)
    {
        if (property.PropertyType != typeof(DateTime) &&
            property.PropertyType != typeof(DateTime?))
        {
            return;
        }

        if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(member) == null)
        {
            property.Converter = _dateTimeConverter.Value;
        }
    }

    protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
    {
        var contract = base.CreateDictionaryContract(objectType);

        contract.DictionaryKeyResolver = propertyName => propertyName;

        return contract;
    }
}
