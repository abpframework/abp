using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.ChartJs;
using Volo.Abp.Modularity;

namespace DashboardDemo.Web.Pages.Shared.Components.NewUserStatisticWidget
{
    [DependsOn(typeof(ChartjsScriptContributor))]
    public class NewUserStatisticWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/Shared/Components/NewUserStatisticWidget/Default.js");
        }
    }
}
