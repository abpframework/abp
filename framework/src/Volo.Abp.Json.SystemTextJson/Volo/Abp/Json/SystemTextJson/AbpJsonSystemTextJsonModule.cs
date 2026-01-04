using System;
using System.Text.Encodings.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson;

[DependsOn(typeof(AbpJsonAbstractionsModule), typeof(AbpTimingModule), typeof(AbpDataModule))]
public class AbpJsonSystemTextJsonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpOptions<AbpSystemTextJsonSerializerOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
            {
                // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
                options.JsonSerializerOptions.Encoder ??= JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new AbpNullableStringToGuidConverter());
                options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

                options.JsonSerializerOptions.TypeInfoResolver = new AbpDefaultJsonTypeInfoResolver(rootServiceProvider
                    .GetRequiredService<IOptions<AbpSystemTextJsonSerializerModifiersOptions>>());

                var dateTimeConverter = rootServiceProvider.GetRequiredService<AbpDateTimeConverter>().SkipDateTimeNormalization();
                var nullableDateTimeConverter = rootServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>().SkipDateTimeNormalization();

                options.JsonSerializerOptions.TypeInfoResolver.As<AbpDefaultJsonTypeInfoResolver>().Modifiers.Add(
                    new AbpDateTimeConverterModifier(dateTimeConverter, nullableDateTimeConverter)
                        .CreateModifyAction());
            });
    }
}
