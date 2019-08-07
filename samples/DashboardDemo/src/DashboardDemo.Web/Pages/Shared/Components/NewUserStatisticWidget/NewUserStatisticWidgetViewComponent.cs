using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.ChartJs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Shared.Components.NewUserStatisticWidget
{
    [Widget(
        StyleTypes = new[] { typeof(ChartjsStyleContributor) },
        ScriptTypes = new[] { typeof(NewUserStatisticWidgetScriptContributor)}
        )]
    public class NewUserStatisticWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
