using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Clipboard;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class MyWidgetViewComponent : ViewComponent
    {
        public const string WidgetName = "MyWidget";
        
        public const string DisplayName = "MyWidgett";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/MyWidgetViewComponent.cshtml", new MyWidgetViewComponent());
        }
    }

    [DependsOn(typeof(JQueryScriptContributor))]
    [DependsOn(typeof(ClipboardScriptBundleContributor))]
    public class MyWidgetViewComponentScriptBundleContributor : BundleContributor
    {

    }
}