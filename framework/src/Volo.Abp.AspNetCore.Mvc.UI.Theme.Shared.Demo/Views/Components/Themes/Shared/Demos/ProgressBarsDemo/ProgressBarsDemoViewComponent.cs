using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.ProgressBarsDemo
{
    [Widget(
        StyleTypes = new []{ typeof(PrismjsStyleBundleContributor) },
        ScriptTypes = new[]{ typeof(PrismjsScriptBundleContributor) }
    )]
    public class ProgressBarsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/ProgressBarsDemo/Default.cshtml";

        public virtual IViewComponentResult Invoke()
        {
            return View(ViewPath);
        }
    }
}