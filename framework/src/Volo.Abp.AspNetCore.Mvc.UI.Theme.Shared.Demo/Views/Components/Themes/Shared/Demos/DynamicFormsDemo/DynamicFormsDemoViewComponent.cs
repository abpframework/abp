using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.DynamicFormsDemo
{
    [Widget]
    public class DynamicFormsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/DynamicFormsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            var model = new DynamicFormsDemoModel();

            return View(ViewPath, model);
        }
    }
}