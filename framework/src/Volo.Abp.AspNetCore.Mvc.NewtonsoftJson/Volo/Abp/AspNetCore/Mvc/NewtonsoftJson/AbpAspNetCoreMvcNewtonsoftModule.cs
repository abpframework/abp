using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AbpAspNetCoreMvcNewtonsoftModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions<AbpAspNetCoreMvcNewtonsoftOptions>();

        if (options.UseHybridSerializer)
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
        }
        else
        {
            context.Services.AddMvcCore().AddNewtonsoftJson();
        }

        context.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, AbpMvcNewtonsoftJsonOptionsSetup>());
    }
}
