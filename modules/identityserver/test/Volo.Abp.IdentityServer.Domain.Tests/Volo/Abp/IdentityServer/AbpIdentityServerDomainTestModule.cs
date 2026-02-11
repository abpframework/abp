using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.IdentityServer;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpIdentityServerTestEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementDomainIdentityServerModule)
)]
public class AbpIdentityServerDomainTestModule : AbpModule
{

}
