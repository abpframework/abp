using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Dapr;

[DependsOn(typeof(AbpJsonModule))]
public class AbpDaprModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpDaprOptions>(configuration.GetSection("Dapr"));
    }
}
