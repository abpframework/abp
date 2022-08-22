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
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((_, clientBuilder) =>
            {
                clientBuilder.AddHttpMessageHandler<AbpInvocationHandler>();
            });
        });
    }
}