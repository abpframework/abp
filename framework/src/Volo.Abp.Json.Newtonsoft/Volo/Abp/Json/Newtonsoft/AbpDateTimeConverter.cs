using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpDateTimeConverter : DateTimeConverterBase, ITransientDependency
{
    private readonly string _dateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
    private readonly DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private readonly IClock _clock;
    private readonly AbpJsonOptions _options;

    public AbpDateTimeConverter(IClock clock, IOptions<AbpJsonOptions> options)
    {
        _clock = clock;
        _options = options.Value;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var nullable = Nullable.GetUnderlyingType(objectType) != null;
        if (reader.TokenType == JsonToken.Null)
        {
            if (!nullable)
            {
                throw new JsonSerializationException($"Cannot convert null value to {objectType.FullName}.");
            }

            return null;
        }

        if (reader.TokenType == JsonToken.Date)
        {
            return _clock.Normalize(reader.Value.To<DateTime>());
        }

        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException($"Unexpected token parsing date. Expected String, got {reader.TokenType}.");
        }

        var dateText = reader.Value?.ToString();

        if (dateText.IsNullOrEmpty() && nullable)
        {
            return null;
        }

        if (_options.InputDateTimeFormats.Any())
        {
            foreach (var format in _options.InputDateTimeFormats)
            {
                if (DateTime.TryParseExact(dateText, format, _culture, _dateTimeStyles, out var d1))
                {
                    return _clock.Normalize(d1);
                }
            }
        }

        var date = !_dateTimeFormat.IsNullOrEmpty() ?
            DateTime.ParseExact(dateText, _dateTimeFormat, _culture, _dateTimeStyles) :
            DateTime.Parse(dateText, _culture, _dateTimeStyles);

        return _clock.Normalize(date);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value != null)
        {
            value = _clock.Normalize(value.To<DateTime>());
        }

        if (value is DateTime dateTime)
        {
            if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal ||
                (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            writer.WriteValue(_options.OutputDateTimeFormat.IsNullOrWhiteSpace()
                ? dateTime.ToString(_dateTimeFormat, _culture)
                : dateTime.ToString(_options.OutputDateTimeFormat, _culture));
        }
        else
        {
            throw new JsonSerializationException($"Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {value.GetType()}.");
        }
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
