using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsAdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class DocsAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(DocsAdminApplicationContractsModule).Assembly, DocsAdminRemoteServiceConsts.RemoteServiceName);
        }
    }
}
