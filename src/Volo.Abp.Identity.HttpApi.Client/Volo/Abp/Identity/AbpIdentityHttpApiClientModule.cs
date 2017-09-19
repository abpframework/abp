using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityApplicationContractsModule), typeof(AbpHttpClientModule))]
    public class AbpIdentityHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AbpIdentity";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityHttpApiClientModule>();

            services.AddHttpClientProxies(typeof(AbpIdentityApplicationContractsModule).Assembly, RemoteServiceName);
        }
    }
}