using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(FeatureManagementApplicationContractsModule),
        typeof(IdentityApplicationContractsModule),
        typeof(PermissionManagementApplicationContractsModule),
        typeof(TenantManagementApplicationContractsModule)
    )]
    public class MyProjectNameApplicationContractsModule : AbpModule
    {

    }
}
