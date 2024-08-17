using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.Modifiers;

public class AbpDateTimeConverterModifier
{
    private readonly AbpDateTimeConverter _abpDateTimeConverter;
    private readonly AbpNullableDateTimeConverter _abpNullableDateTimeConverter;

    public AbpDateTimeConverterModifier(AbpDateTimeConverter abpDateTimeConverter, AbpNullableDateTimeConverter abpNullableDateTimeConverter)
    {
        _abpDateTimeConverter = abpDateTimeConverter;
        _abpNullableDateTimeConverter = abpNullableDateTimeConverter;
    }

    public Action<JsonTypeInfo> CreateModifyAction()
    {
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
                    ? _abpDateTimeConverter
                    : _abpNullableDateTimeConverter;
            }
        }
    }
}
