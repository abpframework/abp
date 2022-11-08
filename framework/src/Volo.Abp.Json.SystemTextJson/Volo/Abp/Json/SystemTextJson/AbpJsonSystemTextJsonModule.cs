using System;
using System.Text.Encodings.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson;

[DependsOn(typeof(AbpJsonAbstractionsModule), typeof(AbpTimingModule))]
public class AbpJsonSystemTextJsonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<AbpSystemTextJsonSerializerOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
                options.JsonSerializerOptions.Encoder ??= JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());
                options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                options.JsonSerializerOptions.TypeInfoResolver = new AbpDefaultJsonTypeInfoResolver(serviceProvider
                    .GetRequiredService<IOptions<AbpSystemTextJsonSerializerModifiersOptions>>());
            });

        context.Services.AddOptions<AbpSystemTextJsonSerializerModifiersOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                options.Modifiers.Add(new AbpDateTimeConverterModifier().CreateModifyAction(serviceProvider));
            });
    }
}
