using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.AzureServiceBus;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpThreadingModule)
)]
public class AbpAzureServiceBusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpAzureServiceBusOptions>(configuration.GetSection("Azure:ServiceBus"));
    }
}
