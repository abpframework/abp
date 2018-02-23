using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.Authorization
{
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            services.AddAssemblyOf<AbpAuthorizationModule>();
        }
    }
}
