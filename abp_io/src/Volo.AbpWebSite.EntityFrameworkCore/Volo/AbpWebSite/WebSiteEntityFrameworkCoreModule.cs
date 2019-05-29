using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.AbpWebSite.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.AbpWebSite
{
    [DependsOn(
        typeof(WebSiteDomainModule),
        typeof(EntityFrameworkCoreSqlServerModule),
        typeof(SettingManagementEntityFrameworkCoreModule),
        typeof(PermissionManagementEntityFrameworkCoreModule),
        typeof(IdentityEntityFrameworkCoreModule),
        typeof(DocsEntityFrameworkCoreModule),
        typeof(BloggingEntityFrameworkCoreModule)
        )]
    public class WebSiteEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpWebSiteDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
        }
    }
}