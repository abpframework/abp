using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.JsonConverters;

namespace Volo.Abp.Json
{
    public class AbpJsonSerializerOptionsSetup : IConfigureOptions<AbpJsonSerializerOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public AbpJsonSerializerOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(AbpJsonSerializerOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>());
        }
    }
}
