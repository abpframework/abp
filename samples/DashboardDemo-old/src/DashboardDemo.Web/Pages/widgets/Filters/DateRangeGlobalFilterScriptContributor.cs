using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace DashboardDemo.Pages.widgets.Filters
{
    public class DateRangeGlobalFilterScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/Filters/DateRangeGlobalFilter.js");
        }
    }
}
