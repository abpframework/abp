using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

    public ILogger<AbpDateTimeConverter> Logger { get; set; }

    private readonly DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private readonly IClock _clock;
    private readonly AbpJsonOptions _options;
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;
    private readonly ITimezoneProvider _timezoneProvider;
    private bool _skipDateTimeNormalization;

    public AbpDateTimeConverter(IClock clock, IOptions<AbpJsonOptions> options, ICurrentTimezoneProvider currentTimezoneProvider, ITimezoneProvider timezoneProvider)
    {
        Logger = NullLogger<AbpDateTimeConverter>.Instance;

        _clock = clock;
        _currentTimezoneProvider = currentTimezoneProvider;
        _timezoneProvider = timezoneProvider;
        _options = options.Value;
    }

    public virtual AbpDateTimeConverter SkipDateTimeNormalization()
    {
        _skipDateTimeNormalization = true;
        return this;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var nullable = Nullable.GetUnderlyingType(objectType) != null;
        switch (reader.TokenType)
        {
            case JsonToken.Null when !nullable:
                throw new JsonSerializationException($"Cannot convert null value to {objectType.FullName}.");
            case JsonToken.Null:
                return null;
            case JsonToken.Date:
                return Normalize(reader.Value!.To<DateTime>());
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
                    return Normalize(d1);
                }
            }
        }

        var date = DateTime.Parse(dateText!, _culture, _dateTimeStyles);
        return Normalize(date);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value != null)
        {
            value = Normalize(value.To<DateTime>());
        }

        if (value is DateTime dateTime)
        {
            if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal ||
                (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            writer.WriteValue(_options.OutputDateTimeFormat.IsNullOrWhiteSpace()
                ? dateTime.ToString(DefaultDateTimeFormat, _culture)
                : dateTime.ToString(_options.OutputDateTimeFormat, _culture));
        }
        else
        {
            throw new JsonSerializationException($"Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {value?.GetType()}.");
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

    protected virtual DateTime Normalize(DateTime dateTime)
    {
        if (dateTime.Kind != DateTimeKind.Unspecified ||
            !_clock.SupportsMultipleTimezone ||
            _currentTimezoneProvider.TimeZone.IsNullOrWhiteSpace())
        {
            return _skipDateTimeNormalization ? dateTime : _clock.Normalize(dateTime);
        }

        try
        {
            var timezoneInfo = _timezoneProvider.GetTimeZoneInfo(_currentTimezoneProvider.TimeZone);
            dateTime = new DateTimeOffset(dateTime, timezoneInfo.GetUtcOffset(dateTime)).UtcDateTime;
        }
        catch
        {
            Logger.LogWarning("Could not convert DateTime with unspecified Kind using timezone '{TimeZone}'.", _currentTimezoneProvider.TimeZone);
        }

        return _skipDateTimeNormalization
            ? dateTime
            : _clock.Normalize(dateTime);
    }
}
