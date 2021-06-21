using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Admin
{
    [DependsOn(
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(CmsKitCommonHttpApiClientModule))]
    public class CmsKitAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CmsKitAdminApplicationContractsModule).Assembly,
                CmsKitAdminRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
