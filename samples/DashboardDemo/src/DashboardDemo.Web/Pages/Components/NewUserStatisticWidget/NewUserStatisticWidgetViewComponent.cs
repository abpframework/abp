using System;
using System.Threading.Tasks;
using DashboardDemo.Web.Bundles;
using DashboardDemo.Web.Pages.Components.NewUserStatisticWidget;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.CountersWidget
{
    [Widget(
        StyleTypes = new[] { typeof(ChartjsStyleContributor) },
        ScriptTypes = new[] { typeof(NewUserStatisticWidgetScriptContributor)}
        )]
    public class NewUserStatisticWidgetViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
