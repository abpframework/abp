using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(IdentityHttpApiModule),
        typeof(PermissionManagementHttpApiModule),
        typeof(TenantManagementHttpApiModule),
        typeof(FeatureManagementHttpApiModule)
        )]
    public class MyProjectNameHttpApiModule : AbpModule
    {
        
    }
}
