using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.PaginatorDemo;

[Widget(
    StyleTypes = new[] { typeof(PrismjsStyleBundleContributor) },
    ScriptTypes = new[] { typeof(PrismjsScriptBundleContributor) }
)]
public class PaginatorDemoViewComponent : AbpViewComponent
{
    public const string ViewPath = "/Views/Components/Themes/Shared/Demos/PaginatorDemo/Default.cshtml";

    public PagerModel PagerModel { get; set; }

    public IViewComponentResult Invoke(PagerModel pagerModel)
    {
        PagerModel = pagerModel;

        return View(ViewPath);
    }
}
