using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [DependsOn(typeof(ClipboardScriptBundleContributor))]
    public class DemoStatisticsScriptContributor : BundleContributor
    {
    }
}
