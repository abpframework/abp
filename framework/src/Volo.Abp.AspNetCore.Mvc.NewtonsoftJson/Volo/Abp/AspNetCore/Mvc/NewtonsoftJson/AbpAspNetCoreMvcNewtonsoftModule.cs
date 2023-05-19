using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

[DependsOn(typeof(AbpJsonNewtonsoftModule), typeof(AbpAspNetCoreMvcModule))]
public class AbpAspNetCoreMvcNewtonsoftModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMvcCore().AddNewtonsoftJson();

        context.Services.AddOptions<MvcNewtonsoftJsonOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
            {
                options.SerializerSettings.ContractResolver = new AbpCamelCasePropertyNamesContractResolver(rootServiceProvider.GetRequiredService<AbpDateTimeConverter>());
            });
    }
}
