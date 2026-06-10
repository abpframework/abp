using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public class AbpNullableDateTimeConverter : AbpDateTimeConverterBase<DateTime?>, ITransientDependency
{
    public AbpNullableDateTimeConverter(
        IClock clock,
        IOptions<AbpJsonOptions> abpJsonOptions,
        ICurrentTimezoneProvider currentTimezoneProvider,
        ITimezoneProvider timezoneProvider)
        : base(clock, abpJsonOptions, currentTimezoneProvider, timezoneProvider)
    {
    }

    public virtual AbpNullableDateTimeConverter SkipDateTimeNormalization()
    {
        IsSkipDateTimeNormalization = true;
        return this;
    }

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Options.InputDateTimeFormats.Any() && reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Reader's TokenType is not String!");
        }

        if (TryReadDateTime(ref reader, out var result))
        {
            return result;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        WriteDateTime(writer, value.Value);
    }
}
