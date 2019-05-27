using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace VoloDocs.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocsEntityFrameworkCoreModule),
        typeof(IdentityEntityFrameworkCoreModule),
        typeof(PermissionManagementEntityFrameworkCoreModule),
        typeof(SettingManagementEntityFrameworkCoreModule),
        typeof(EntityFrameworkCoreSqlServerModule))]
    public class VoloDocsEntityFrameworkCoreModule : AbpModule
    {
        
    }
}
