using Volo.Abp.Modularity;

namespace DashboardDemo
{
    [DependsOn(
        typeof(DashboardDemoApplicationModule),
        typeof(DashboardDemoDomainTestModule)
        )]
    public class DashboardDemoApplicationTestModule : AbpModule
    {

    }
}