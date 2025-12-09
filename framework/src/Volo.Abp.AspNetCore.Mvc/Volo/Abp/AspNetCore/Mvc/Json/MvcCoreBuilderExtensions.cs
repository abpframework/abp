using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Json.SystemTextJson.Modifiers;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public static class MvcCoreBuilderExtensions
{
    public static IMvcCoreBuilder AddAbpJson(this IMvcCoreBuilder builder)
    {
        builder.Services.AddAbpOptions<JsonOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
            {
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                options.JsonSerializerOptions.AllowTrailingCommas = true;

                options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpNullableStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                options.JsonSerializerOptions.TypeInfoResolver = new AbpDefaultJsonTypeInfoResolver(rootServiceProvider
                    .GetRequiredService<IOptions<AbpSystemTextJsonSerializerModifiersOptions>>());

                var dateTimeConverter = rootServiceProvider.GetRequiredService<AbpDateTimeConverter>();
                var nullableDateTimeConverter = rootServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>();

                options.JsonSerializerOptions.TypeInfoResolver.As<AbpDefaultJsonTypeInfoResolver>().Modifiers.Add(
                    new AbpDateTimeConverterModifier(dateTimeConverter, nullableDateTimeConverter)
                        .CreateModifyAction());
            });

        return builder;
    }
}
