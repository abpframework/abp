using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.Newtonsoft;

[DependsOn(typeof(AbpJsonAbstractionsModule), typeof(AbpTimingModule))]
public class AbpJsonNewtonsoftModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<AbpNewtonsoftJsonSerializerOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                options.JsonSerializerSettings.ContractResolver = new AbpCamelCasePropertyNamesContractResolver(serviceProvider.GetRequiredService<AbpDateTimeConverter>());
            });
    }
}
