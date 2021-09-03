using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitCommonHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(CmsKitCommonApplicationContractsModule).Assembly,
                CmsKitCommonRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
