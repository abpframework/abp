using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    [DependsOn(typeof(AbpDddModule))]
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var identityBuilder = services.AddAbpIdentity();
            services.ExecutePreConfiguredActions(identityBuilder);

            services.AddAssemblyOf<AbpIdentityDomainModule>();
        }
    }
}