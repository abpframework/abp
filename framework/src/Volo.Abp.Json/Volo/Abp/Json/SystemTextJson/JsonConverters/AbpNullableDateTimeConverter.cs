using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpNullableDateTimeConverter : JsonConverter<DateTime?>, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly AbpJsonOptions _options;

        public AbpNullableDateTimeConverter(IClock clock, IOptions<AbpJsonOptions> abpJsonOptions)
        {
            _clock = clock;
            _options = abpJsonOptions.Value;
        }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetDateTime(out var dateTime))
            {
                return  _clock.Normalize(dateTime);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                if (_options.DefaultDateTimeFormat.IsNullOrWhiteSpace())
                {
                    writer.WriteStringValue(_clock.Normalize(value.Value));
                }
                else
                {
                    writer.WriteStringValue(_clock.Normalize(value.Value).ToString(_options.DefaultDateTimeFormat));
                }
            }
        }
    }
}
