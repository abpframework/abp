using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSerializerOptionsSetup : IConfigureOptions<AbpSystemTextJsonSerializerOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public AbpSystemTextJsonSerializerOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(AbpSystemTextJsonSerializerOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>());

            options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
            options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());

            options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

            // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
            options.JsonSerializerOptions.Encoder ??= JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }
    }
}
