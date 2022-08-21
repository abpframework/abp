using System;
using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpJsonIsoDateTimeConverter : IsoDateTimeConverter, ITransientDependency
{
    private readonly IClock _clock;

    public AbpJsonIsoDateTimeConverter(IClock clock, IOptions<AbpJsonOptions> abpJsonOptions)
    {
        _clock = clock;

        if (abpJsonOptions.Value.DefaultDateTimeFormat != null)
        {
            DateTimeFormat = abpJsonOptions.Value.DefaultDateTimeFormat;
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var date = base.ReadJson(reader, objectType, existingValue, serializer) as DateTime?;

        if (date.HasValue)
        {
            return _clock.Normalize(date.Value);
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var date = value as DateTime?;
        base.WriteJson(writer, date.HasValue ? _clock.Normalize(date.Value) : value, serializer);
    }

    internal static bool ShouldNormalize(MemberInfo member, JsonProperty property)
    {
        if (property.PropertyType != typeof(DateTime) &&
            property.PropertyType != typeof(DateTime?))
        {
            return false;
        }

        return ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(member) == null;
    }
}
