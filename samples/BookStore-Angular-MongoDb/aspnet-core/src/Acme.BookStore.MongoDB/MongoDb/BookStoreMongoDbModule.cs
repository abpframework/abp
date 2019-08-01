using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BackgroundJobs.MongoDB;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Abp.TenantManagement.MongoDB;

namespace Acme.BookStore.MongoDB
{
    [DependsOn(
        typeof(BookStoreDomainModule),
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpIdentityMongoDbModule),
        typeof(AbpIdentityServerMongoDbModule),
        typeof(BackgroundJobsMongoDbModule),
        typeof(AbpAuditLoggingMongoDbModule),
        typeof(AbpTenantManagementMongoDbModule),
        typeof(AbpFeatureManagementMongoDbModule)
        )]
    public class BookStoreMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
        }
    }
}
