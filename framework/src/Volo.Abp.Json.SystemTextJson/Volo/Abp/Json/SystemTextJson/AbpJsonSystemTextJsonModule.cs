using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson;

[DependsOn(typeof(AbpJsonCoreModule), typeof(AbpTimingModule))]
public class AbpJsonSystemTextJsonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<AbpSystemTextJsonSerializerOptions>, AbpSystemTextJsonSerializerOptionsSetup>());

        var preActions = context.Services.GetPreConfigureActions<AbpJsonOptions>();
        Configure<AbpJsonOptions>(options =>
        {
            if (preActions.Configure().UseHybridSerializer)
            {
                options.Providers.Add<AbpSystemTextJsonSerializerProvider>();
            }
        });
    }
}
