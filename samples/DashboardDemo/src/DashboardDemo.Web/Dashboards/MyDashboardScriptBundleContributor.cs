using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.Modularity;

namespace DashboardDemo.Dashboards
{
    [DependsOn(
        typeof(AbpBasicDashboardScriptContributor),
        typeof(MyWidgetViewComponentScriptBundleContributor),
        typeof(DemoStatisticsScriptContributor)
    )]
    public class MyDashboardScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {

        }
    }
}