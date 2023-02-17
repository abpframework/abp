using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public static class MvcCoreBuilderExtensions
{
    public static IMvcCoreBuilder AddAbpJson(this IMvcCoreBuilder builder)
    {
        builder.Services.AddOptions<JsonOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                options.JsonSerializerOptions.AllowTrailingCommas = true;

                options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpNullableStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                options.JsonSerializerOptions.TypeInfoResolver = new AbpDefaultJsonTypeInfoResolver(serviceProvider
                    .GetRequiredService<IOptions<AbpSystemTextJsonSerializerModifiersOptions>>());
            });

        return builder;
    }
}
