using DashboardDemo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace DashboardDemo
{
    [DependsOn(
        typeof(DashboardDemoEntityFrameworkCoreTestModule)
        )]
    public class DashboardDemoDomainTestModule : AbpModule
    {

    }
}