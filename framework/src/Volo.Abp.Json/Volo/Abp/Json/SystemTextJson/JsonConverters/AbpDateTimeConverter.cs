using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public class AbpDateTimeConverter : JsonConverter<DateTime>, ITransientDependency
{
    private readonly IClock _clock;
    private readonly AbpJsonOptions _options;

    public AbpDateTimeConverter(IClock clock, IOptions<AbpJsonOptions> abpJsonOptions)
    {
        _clock = clock;
        _options = abpJsonOptions.Value;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!_options.DefaultDateTimeFormat.IsNullOrWhiteSpace())
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (DateTime.TryParseExact(s, _options.DefaultDateTimeFormat, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d1))
                {
                    return _clock.Normalize(d1);
                }

                throw new JsonException($"'{s}' can't parse to DateTime({_options.DefaultDateTimeFormat})!");
            }

            throw new JsonException("Reader's TokenType is not String!");
        }

        if (reader.TryGetDateTime(out var d2))
        {
            return _clock.Normalize(d2);
        }

        throw new JsonException("Can't get datetime from the reader!");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (_options.DefaultDateTimeFormat.IsNullOrWhiteSpace())
        {
            writer.WriteStringValue(_clock.Normalize(value));
        }
        else
        {
            writer.WriteStringValue(_clock.Normalize(value).ToString(_options.DefaultDateTimeFormat, CultureInfo.CurrentUICulture));
        }
    }
}
