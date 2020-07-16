using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class CmsKitHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CmsKit";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CmsKitApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
