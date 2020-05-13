using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class DocsHttpApiClientModule : AbpModule
    {
        //TODO: Create client proxies
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(DocsApplicationContractsModule).Assembly);
        }
    }
}
