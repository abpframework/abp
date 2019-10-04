using DashboardDemo.Pages.widgets.Chartjs;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(BootstrapStyleContributor))]
    [DependsOn(typeof(ChartjsStyleContributor))]
    public class MonthlyProfitWidgetStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/MonthlyProfitWidget/MonthlyProfitWidget.css");
        }
    }
}
