using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
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
            return  _clock.Normalize(reader.GetDateTime());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (_options.DefaultDateTimeFormat.IsNullOrWhiteSpace())
            {
                writer.WriteStringValue(_clock.Normalize(value));
            }
            else
            {
                writer.WriteStringValue(_clock.Normalize(value).ToString(_options.DefaultDateTimeFormat));
            }
        }
    }
}
