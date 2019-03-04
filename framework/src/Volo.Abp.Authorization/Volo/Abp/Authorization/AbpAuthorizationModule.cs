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
        typeof(AbpLocalizationAbstractionsModule)
        )]
    public class AbpAuthorizationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
            //TODO: Auto Add Providers to PermissionOptions just like did in AbpFeaturesModule.AutoAddProviders
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();

            context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            Configure<PermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
                options.ValueProviders.Add<RolePermissionValueProvider>();
                options.ValueProviders.Add<ClientPermissionValueProvider>();
            });
        }
    }
}
