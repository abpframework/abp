using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.Client;

[DependsOn(
    typeof(AbpAspNetCoreMvcClientCommonModule),
    typeof(AbpEventBusModule)
    )]
public class AbpAspNetCoreMvcClientModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider.GetRequiredService<MvcCachedApplicationConfigurationClient>().InitializeAsync();
    }
}
