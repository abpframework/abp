using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyApplicationContractsModule), typeof(AbpHttpClientModule))]
    public class AbpMultiTenancyHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpMultiTenancy";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClientProxies(
                typeof(AbpMultiTenancyApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            services.AddAssemblyOf<AbpMultiTenancyHttpApiClientModule>();
        }
    }
}