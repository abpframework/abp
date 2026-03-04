using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace DistDemoApp;

[DependsOn(
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(DistDemoAppSharedModule)
)]
public class DistDemoAppEntityFrameworkCoreInfrastructureModule : AbpModule
{
}
