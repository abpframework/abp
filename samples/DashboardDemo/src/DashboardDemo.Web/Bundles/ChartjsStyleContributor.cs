using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace DashboardDemo.Web.Bundles
{
    public class ChartjsStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/chart.js/Chart.css");
        }
    }
}
