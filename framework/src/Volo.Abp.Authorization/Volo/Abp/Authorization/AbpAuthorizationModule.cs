using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.Authorization
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpLocalizationModule)
        )]
    public class AbpAuthorizationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();

            context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            context.Services.Configure<PermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
                options.ValueProviders.Add<RolePermissionValueProvider>();
            });

            context.Services.AddAssemblyOf<AbpAuthorizationModule>();
        }
    }
}
