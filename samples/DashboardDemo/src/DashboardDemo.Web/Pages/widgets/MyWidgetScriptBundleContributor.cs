using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(JQueryScriptContributor))]
    [DependsOn(typeof(ClipboardScriptBundleContributor))]
    public class MyWidgetScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/widgets/MyDashboard.js");
        }
    }
}