using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.Json
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
        }
    }
}
