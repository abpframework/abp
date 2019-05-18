using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.MultiTenancy;
using Volo.Abp;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Threading;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(BackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule)
        )]
    public class MyProjectNameDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<MultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsMultiTenancyEnabled;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedDatabase(context);
        }

        private void SeedDatabase(ApplicationInitializationContext context)
        {
            /* Seeding in the application startup can be a problem in a clustered environment.
             * See https://github.com/abpframework/abp/issues/1123
             */
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(async () =>
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                });
            }
        }
    }
}
