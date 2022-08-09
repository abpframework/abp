using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.Dapr;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpDaprModule)
)]
public class AbpHttpClientDaprModule : AbpModule
{ 
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var daprOptions = context.Services.ExecutePreConfiguredActions<AbpDaprOptions>();
        var daprClientOptions = context.Services.ExecutePreConfiguredActions<AbpDaprClientOptions>();

        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((_, clientBuilder) =>
            {
                daprClientOptions.DaprHttpClientBuilderAction(_, clientBuilder, daprOptions.HttpEndpoint);
            });
        });
    }
}