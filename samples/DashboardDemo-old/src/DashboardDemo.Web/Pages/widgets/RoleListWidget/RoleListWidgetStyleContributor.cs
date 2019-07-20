using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace DashboardDemo.Pages.widgets
{
    public class RoleListWidgetStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/RoleListWidget/RoleListWidget.css");
        }
    }
}
