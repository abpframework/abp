using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpPermissionsDomainModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    [DependsOn(typeof(AbpUsersModule))]
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