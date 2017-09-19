using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityApplicationContractsModule), typeof(AbpHttpClientModule))]
    public class AbpIdentityHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityHttpApiClientModule>();

            services.AddHttpClientProxy<AbpIdentityApplicationContractsModule>("AbpIdentity");
        }
    }
}