using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public class AbpDateTimeConverter : AbpDateTimeConverterBase<DateTime>, ITransientDependency
{
    public AbpDateTimeConverter(
        IClock clock,
        IOptions<AbpJsonOptions> abpJsonOptions,
        ICurrentTimezoneProvider currentTimezoneProvider,
        ITimezoneProvider timezoneProvider)
        : base(clock, abpJsonOptions, currentTimezoneProvider, timezoneProvider)
    {
    }

    public virtual AbpDateTimeConverter SkipDateTimeNormalization()
    {
        IsSkipDateTimeNormalization = true;
        return this;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Options.InputDateTimeFormats.Any() && reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Reader's TokenType is not String!");
        }

        return TryReadDateTime(ref reader, out var result)
            ? result
            : throw new JsonException("Can't get datetime from the reader!");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        WriteDateTime(writer, value);
    }
}
