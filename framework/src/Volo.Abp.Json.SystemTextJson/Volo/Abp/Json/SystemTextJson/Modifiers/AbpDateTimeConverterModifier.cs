using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.Modifiers;

public class AbpDateTimeConverterModifier
{
    private IServiceProvider _serviceProvider;

    public Action<JsonTypeInfo> CreateModifyAction(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return Modify;
    }

    private void Modify(JsonTypeInfo jsonTypeInfo)
    {
        if (ReflectionHelper.GetAttributesOfMemberOrDeclaringType<DisableDateTimeNormalizationAttribute>(jsonTypeInfo.Type).Any())
        {
            return;
        }

        foreach (var property in jsonTypeInfo.Properties.Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)))
        {
            if (property.AttributeProvider == null ||
                !property.AttributeProvider.GetCustomAttributes(typeof(DisableDateTimeNormalizationAttribute), false).Any())
            {
                property.CustomConverter = property.PropertyType == typeof(DateTime)
                    ? _serviceProvider.GetRequiredService<AbpDateTimeConverter>()
                    : _serviceProvider.GetRequiredService<AbpNullableDateTimeConverter>();
            }
        }
    }
}
