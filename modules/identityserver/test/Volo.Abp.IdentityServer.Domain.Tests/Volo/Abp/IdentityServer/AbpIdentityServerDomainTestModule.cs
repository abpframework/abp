using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SecurityLog;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityServerTestEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainIdentityServerModule)
)]
public class AbpIdentityServerDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IdentityUserStoreFailureSimulator>();
        context.Services.AddSingleton<IdentityServerTestSettingValueProvider>();
        context.Services.Replace(ServiceDescriptor.Scoped<IdentityUserStore, TestIdentityUserStore>());
        context.Services.Replace(ServiceDescriptor.Singleton<IUnitOfWorkManager>(
            serviceProvider => serviceProvider.GetRequiredService<UnitOfWorkManager>()));

        Configure<AbpSecurityLogOptions>(options => options.IsEnabled = false);
        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.Add<IdentityServerTestSettingValueProvider>();
        });
    }

}
