using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace DashboardDemo.EntityFrameworkCore
{
    [DependsOn(
        typeof(DashboardDemoEntityFrameworkCoreModule)
        )]
    public class DashboardDemoEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DashboardDemoMigrationsDbContext>();
        }
    }
}
