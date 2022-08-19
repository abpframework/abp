using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AbpAspNetCoreMvcNewtonsoftModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

        Configure<MvcOptions>(mvcOptions =>
        {
            mvcOptions.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
            mvcOptions.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
        });

        Configure<AbpHybridJsonFormatterOptions>(formatterOptions =>
        {
            formatterOptions.TextInputFormatters.Add<AbpNewtonsoftJsonFormatter>();
            formatterOptions.TextOutputFormatters.Add<AbpNewtonsoftJsonFormatter>();
        });

        context.Services.AddOptions<MvcNewtonsoftJsonOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                options.SerializerSettings.ContractResolver = serviceProvider.GetRequiredService<AbpCamelCasePropertyNamesContractResolver>();

                var converters = serviceProvider.GetRequiredService<IOptions<AbpNewtonsoftJsonSerializerOptions>>().Value
                    .Converters.Select(converterType => serviceProvider.GetRequiredService(converterType).As<JsonConverter>())
                    .ToList();

                options.SerializerSettings.Converters.InsertRange(0, converters);
            });
    }
}
