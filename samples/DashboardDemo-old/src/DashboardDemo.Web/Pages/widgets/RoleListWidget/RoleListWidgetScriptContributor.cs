using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(ClipboardScriptBundleContributor))]
    public class RoleListWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/RoleListWidget/RoleListWidget.js");
        }
    }
}
