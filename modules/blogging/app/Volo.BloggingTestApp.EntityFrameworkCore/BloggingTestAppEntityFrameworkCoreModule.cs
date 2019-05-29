using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.BloggingTestApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingEntityFrameworkCoreModule),
        typeof(IdentityEntityFrameworkCoreModule),
        typeof(PermissionManagementEntityFrameworkCoreModule),
        typeof(SettingManagementEntityFrameworkCoreModule),
        typeof(EntityFrameworkCoreSqlServerModule))]
    public class BloggingTestAppEntityFrameworkCoreModule : AbpModule
    {

    }
}
