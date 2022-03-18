using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json;

[DependsOn(typeof(AbpTimingModule))]
public class AbpJsonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<AbpSystemTextJsonSerializerOptions>, AbpSystemTextJsonSerializerOptionsSetup>());

        var preActions = context.Services.GetPreConfigureActions<AbpJsonOptions>();
        Configure<AbpJsonOptions>(options =>
        {
            options.Providers.Add<AbpNewtonsoftJsonSerializerProvider>();
            if (preActions.Configure().UseHybridSerializer)
            {
                options.Providers.Add<AbpSystemTextJsonSerializerProvider>();
            }
        });

        Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        {
            options.Converters.Add<AbpJsonIsoDateTimeConverter>();
        });
    }
}
