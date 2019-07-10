using DashboardDemo.Pages.widgets.Chartjs;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(JQueryScriptContributor))]
    [DependsOn(typeof(ClipboardScriptBundleContributor))]
    [DependsOn(typeof(ChartjsScriptContributor))]
    [DependsOn(typeof(DateRangeGlobalFilterScriptContributor))]
    public class UserCountWidgetScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/UserCountWidget.js");
        }
    }
}