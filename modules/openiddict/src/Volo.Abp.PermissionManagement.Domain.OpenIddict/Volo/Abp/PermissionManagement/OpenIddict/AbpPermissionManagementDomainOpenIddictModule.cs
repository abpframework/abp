using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.PermissionManagement.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpPermissionManagementDomainModule)
)]
public class AbpPermissionManagementDomainOpenIddictModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<ApplicationPermissionManagementProvider>();
            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "OpenIddictPro.Application.ManagePermissions";
        });

        context.Services.AddAbpOptions<PermissionManagementOptions>().PostConfigure<IServiceProvider>((options, serviceProvider) =>
        {
            // The IApplicationFinder implementation in OpenIddict Pro module for tiered application.
            if (serviceProvider.GetService<IApplicationFinder>() == null)
            {
                return;
            }

            options.ResourceManagementProviders.Add<ApplicationResourcePermissionManagementProvider>();
            options.ResourcePermissionProviderKeyLookupServices.Add<ApplicationResourcePermissionProviderKeyLookupService>();
        });
    }
}
