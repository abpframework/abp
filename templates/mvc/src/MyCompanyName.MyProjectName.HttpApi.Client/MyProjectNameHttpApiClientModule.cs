using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(IdentityHttpApiClientModule),
        typeof(PermissionManagementHttpApiClientModule),
        typeof(TenantManagementHttpApiClientModule),
        typeof(FeatureManagementHttpApiClientModule)
    )]
    public class MyProjectNameHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MyProjectNameApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
