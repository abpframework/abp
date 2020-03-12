using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.TabsDemo
{
    [Widget]
    public class TabsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/TabsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            return View(ViewPath);
        }
    }
}