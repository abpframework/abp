using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.AspNetCore;

[DependsOn(typeof(AbpIdentityAspNetCoreTestModule))]
public class SharedAbpIdentityAspNetCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }
}
