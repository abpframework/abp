using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpPermissionsApplicationContractsModule), typeof(AbpHttpClientModule))]
    public class AbpPermissionsHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpPermissions";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsHttpApiClientModule>();

            services.AddHttpClientProxies(typeof(AbpPermissionsApplicationContractsModule).Assembly, RemoteServiceName);
        }
    }
}