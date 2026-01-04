using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Modularity;

namespace Volo.Abp.Json;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpJsonSystemTextJsonModule),
    typeof(AbpTestBaseModule)
)]
public class AbpJsonSystemTextJsonTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpOptions<AbpSystemTextJsonSerializerOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
            {
                if (options.JsonSerializerOptions.TypeInfoResolver != null)
                {
                    var modifiers = options.JsonSerializerOptions.TypeInfoResolver.As<AbpDefaultJsonTypeInfoResolver>().Modifiers;
                    modifiers.RemoveAll(x => x.Target?.GetType() == typeof(AbpDateTimeConverterModifier));
                    modifiers.Add(new AbpDateTimeConverterModifier(
                        rootServiceProvider.GetRequiredService<SystemTextJson.JsonConverters.AbpDateTimeConverter>(),
                        rootServiceProvider.GetRequiredService<AbpNullableDateTimeConverter>()).CreateModifyAction());
                }
            });
    }
}

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpJsonNewtonsoftModule),
    typeof(AbpTestBaseModule)
)]
public class AbpJsonNewtonsoftTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpOptions<AbpNewtonsoftJsonSerializerOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
            {
                options.JsonSerializerSettings.ContractResolver = new AbpCamelCasePropertyNamesContractResolver(
                    rootServiceProvider.GetRequiredService<Newtonsoft.AbpDateTimeConverter>());
            });
    }
}
