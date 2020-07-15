using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(CmsKitCommonHttpApiClientModule))]
    public class CmsKitAdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CmsKitAdmin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CmsKitAdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
