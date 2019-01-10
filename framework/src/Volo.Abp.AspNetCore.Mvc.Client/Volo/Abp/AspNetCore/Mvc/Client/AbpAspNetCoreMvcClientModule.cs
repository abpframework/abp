using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreMvcContractsModule)
        )]
    public class AbpAspNetCoreMvcClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpMvcClient";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpAspNetCoreMvcContractsModule).Assembly,
                RemoteServiceName,
                asDefaultServices: false
            );
        }
    }
}
