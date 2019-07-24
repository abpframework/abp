using DashboardDemo.Web.Bundles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashboardDemo.Web.Pages.Components.LicenseStatisticWidget
{
    [DependsOn(typeof(ChartjsScriptContributor))]
    public class LicenseStatisticWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/Components/LicenseStatisticWidget/Default.js");
        }
    }
}
