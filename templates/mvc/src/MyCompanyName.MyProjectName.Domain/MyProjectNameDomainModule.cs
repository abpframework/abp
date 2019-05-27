using MyCompanyName.MyProjectName.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(AuditLoggingDomainModule),
        typeof(BackgroundJobsDomainModule),
        typeof(FeatureManagementDomainModule),
        typeof(IdentityDomainModule),
        typeof(PermissionManagementDomainIdentityModule),
        typeof(IdentityServerDomainModule),
        typeof(PermissionManagementDomainIdentityServerModule),
        typeof(SettingManagementDomainModule),
        typeof(TenantManagementDomainModule)
        )]
    public class MyProjectNameDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<MultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }
    }
}
