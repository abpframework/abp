using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace DashboardDemo.Pages.widgets.Filters
{
    public class RefreshGlobalFilterScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/Filters/RefreshGlobalFilter.js");
        }
    }
}
