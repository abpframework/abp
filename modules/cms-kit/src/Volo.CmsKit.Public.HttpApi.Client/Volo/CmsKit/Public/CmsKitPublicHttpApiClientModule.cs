using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public
{
    [DependsOn(
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(CmsKitCommonHttpApiClientModule))]
    public class CmsKitPublicHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CmsKitPublicApplicationContractsModule).Assembly,
                CmsKitPublicRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
