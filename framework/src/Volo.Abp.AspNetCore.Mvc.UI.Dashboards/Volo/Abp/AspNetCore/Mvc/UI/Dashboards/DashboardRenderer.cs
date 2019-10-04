using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards.Components.Dashboard;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardRenderer : IDashboardRenderer
    {
        public async Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, object args = null)
        {
            return await componentHelper.InvokeAsync(typeof(DashboardViewComponent), args ?? new object());
        }
    }
}
