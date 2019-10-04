using DashboardDemo.Pages.widgets;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages
{
    [DependsOn(
        typeof(AbpBasicDashboardScriptContributor),
        typeof(UserCountWidgetScriptBundleContributor),
        typeof(MonthlyProfitWidgetScriptBundleContributor),
        typeof(RoleListWidgetScriptContributor),
        typeof(DateRangeGlobalFilterScriptContributor)
    )]
    public class MyDashboardScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {

        }
    }
}