using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Admin
{
    [DependsOn(
        typeof(AdminApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Admin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
