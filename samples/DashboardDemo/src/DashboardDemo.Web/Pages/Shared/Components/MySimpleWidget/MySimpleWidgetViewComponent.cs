using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Shared.Components.MySimpleWidget
{
    [Widget(
        StyleFiles = new[] { "/Pages/Shared/Components/MySimpleWidget/Default.css" },
        ScriptFiles = new[] { "/Pages/Shared/Components/MySimpleWidget/Default.js" }
    )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke(string name)
        {
            return View(new MySimpleWidgetViewModel { Name = name });
        }
    }
}