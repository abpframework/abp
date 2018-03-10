using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpIdentityServerDomainSharedModule))]
    public class AbpIdentityServerApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityServerApplicationContractsModule>();
        }
    }
}