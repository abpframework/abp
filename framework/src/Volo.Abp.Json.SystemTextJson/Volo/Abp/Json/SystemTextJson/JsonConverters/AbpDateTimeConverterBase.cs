using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public abstract class AbpDateTimeConverterBase<T> : JsonConverter<T>
{
    public ILogger<AbpDateTimeConverterBase<T>> Logger { get; set; }

    protected IClock Clock { get; }
    protected AbpJsonOptions Options { get; }
    protected ICurrentTimezoneProvider CurrentTimezoneProvider { get; }
    protected ITimezoneProvider TimezoneProvider { get; }
    protected bool IsSkipDateTimeNormalization { get; set; }

    protected AbpDateTimeConverterBase(
        IClock clock,
        IOptions<AbpJsonOptions> abpJsonOptions,
        ICurrentTimezoneProvider currentTimezoneProvider,
        ITimezoneProvider timezoneProvider)
    {
        Logger = NullLogger<AbpDateTimeConverterBase<T>>.Instance;

        Clock = clock;
        CurrentTimezoneProvider = currentTimezoneProvider;
        TimezoneProvider = timezoneProvider;
        Options = abpJsonOptions.Value;
    }

    protected bool TryReadDateTime(ref Utf8JsonReader reader, out DateTime value)
    {
        value = default;

        if (Options.InputDateTimeFormats.Any())
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                return false;
            }

            var s = reader.GetString();
            foreach (var format in Options.InputDateTimeFormats)
            {
                if (!DateTime.TryParseExact(s, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d1))
                {
                    continue;
                }

                value = Normalize(d1);
                return true;
            }
        }

        if (reader.TryGetDateTime(out var d2))
        {
            value = Normalize(d2);
            return true;
        }

        var dateText = reader.GetString();
        if (dateText.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (!DateTime.TryParse(dateText, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d3))
        {
            return false;
        }

        value = Normalize(d3);
        return true;

    }

    protected void WriteDateTime(Utf8JsonWriter writer, DateTime value)
    {
        if (Options.OutputDateTimeFormat.IsNullOrWhiteSpace())
        {
            writer.WriteStringValue(Normalize(value));
        }
        else
        {
            writer.WriteStringValue(Normalize(value).ToString(Options.OutputDateTimeFormat, CultureInfo.CurrentUICulture));
        }
    }

    protected virtual DateTime Normalize(DateTime dateTime)
    {
        if (dateTime.Kind != DateTimeKind.Unspecified ||
            !Clock.SupportsMultipleTimezone ||
            CurrentTimezoneProvider.TimeZone.IsNullOrWhiteSpace())
        {
            return IsSkipDateTimeNormalization ? dateTime : Clock.Normalize(dateTime);
        }

        try
        {
            var timezoneInfo = TimezoneProvider.GetTimeZoneInfo(CurrentTimezoneProvider.TimeZone);
            dateTime = new DateTimeOffset(dateTime, timezoneInfo.GetUtcOffset(dateTime)).UtcDateTime;
        }
        catch
        {
            Logger.LogWarning("Could not convert DateTime with unspecified Kind using timezone '{TimeZone}'.", CurrentTimezoneProvider.TimeZone);
        }

        return IsSkipDateTimeNormalization ? dateTime : Clock.Normalize(dateTime);
    }
}
