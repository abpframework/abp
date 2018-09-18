using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BackgroundJobs.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDb
{
    [DependsOn(
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpIdentityMongoDbModule),
        typeof(BackgroundJobsMongoDbModule),
        typeof(AbpAuditLoggingMongoDbModule)
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
