using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages
{
    [DependsOn(
        typeof(AbpBasicDashboardScriptContributor),
        typeof(UserCountWidgetScriptBundleContributor),
        typeof(RoleListWidgetScriptContributor)
    )]
    public class MyDashboardScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {

        }
    }
}