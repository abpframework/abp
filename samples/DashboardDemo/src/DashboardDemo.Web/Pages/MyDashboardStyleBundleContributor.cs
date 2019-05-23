using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages
{
    [DependsOn(
        typeof(AbpBasicDashboardStyleContributor),
        typeof(MyWidgetStyleBundleContributor),
        typeof(DemoStatisticsStyleContributor)
    )]
    public class MyDashboardStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {

        }
    }
}
