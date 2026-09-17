using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Maui.Client;

[DependsOn(
    typeof(AbpAspNetCoreMvcClientCommonModule)
)]
public class AbpMauiClientModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider.GetRequiredService<IClientScopeServiceProviderAccessor>().ServiceProvider.GetRequiredService<MauiCachedApplicationConfigurationClient>().InitializeAsync();
    }
}
