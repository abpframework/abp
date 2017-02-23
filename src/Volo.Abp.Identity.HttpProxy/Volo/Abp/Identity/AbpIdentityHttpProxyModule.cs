using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    //TODO: Rename to Volo.Abp.Identity.HttpApi.Client / AbpIdentityHttpApiClientModule
    [DependsOn(typeof(AbpIdentityApplicationContractsModule))]
    public class AbpIdentityHttpProxyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityHttpProxyModule>();
        }
    }
}