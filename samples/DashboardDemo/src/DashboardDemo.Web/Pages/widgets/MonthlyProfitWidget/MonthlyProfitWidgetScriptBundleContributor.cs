using DashboardDemo.Pages.widgets.Chartjs;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(JQueryScriptContributor))]
    [DependsOn(typeof(ChartjsScriptContributor))]
    public class MonthlyProfitWidgetScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/MonthlyProfitWidget/MonthlyProfitWidget.js");
        }
    }
}