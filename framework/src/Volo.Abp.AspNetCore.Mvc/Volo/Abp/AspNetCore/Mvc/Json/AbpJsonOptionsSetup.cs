using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

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
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.JsonSerializerOptions.AllowTrailingCommas = true;

            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>());

            options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
            options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());

            options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        }
    }
}
