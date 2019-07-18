using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace DashboardDemo.Pages.widgets.Chartjs
{
    public class ChartjsScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/chart.js/Chart.js");
        }
    }
}
