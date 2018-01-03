using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    public class AbpIdentityApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityApplicationContractsModule>();
        }
    }
}