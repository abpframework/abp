using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.Microsoft;
using Volo.Abp.Json.Microsoft.JsonConverters;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpJsonOptionsSetup : IConfigureOptions<JsonOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public AbpJsonOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>());
        }
    }
}
