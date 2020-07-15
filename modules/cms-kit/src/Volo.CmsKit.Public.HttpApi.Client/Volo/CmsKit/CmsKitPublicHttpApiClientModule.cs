using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(CmsKitCommonHttpApiClientModule))]
    public class CmsKitPublicHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CmsKitPublic";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CmsKitPublicApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
