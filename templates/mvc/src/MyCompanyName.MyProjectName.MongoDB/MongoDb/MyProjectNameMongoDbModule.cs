using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BackgroundJobs.MongoDB;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Abp.TenantManagement.MongoDb;

namespace MyCompanyName.MyProjectName.MongoDb
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(PermissionManagementMongoDbModule),
        typeof(SettingManagementMongoDbModule),
        typeof(IdentityMongoDbModule),
        typeof(IdentityServerMongoDbModule),
        typeof(BackgroundJobsMongoDbModule),
        typeof(AuditLoggingMongoDbModule),
        typeof(TenantManagementMongoDbModule),
        typeof(FeatureManagementMongoDbModule)
        )]
    public class MyProjectNameMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<MyProjectNameMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
        }
    }
}
