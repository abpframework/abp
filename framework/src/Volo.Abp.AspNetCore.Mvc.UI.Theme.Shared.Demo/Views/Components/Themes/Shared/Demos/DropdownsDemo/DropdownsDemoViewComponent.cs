using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.DropdownsDemo
{
    [Widget]
    public class DropdownsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/DropdownsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            var Model = new DropDownDemoDemoModel();

            return View(ViewPath, Model);
        }
    }
}