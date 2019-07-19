using DashboardDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DashboardDemo.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(DashboardDemoEntityFrameworkCoreDbMigrationsModule),
        typeof(DashboardDemoApplicationContractsModule)
        )]
    public class DashboardDemoDbMigratorModule : AbpModule
    {
        
    }
}
