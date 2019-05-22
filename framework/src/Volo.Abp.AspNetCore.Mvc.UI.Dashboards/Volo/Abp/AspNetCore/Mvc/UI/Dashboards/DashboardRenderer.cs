using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardRenderer : IDashboardRenderer
    {
        private readonly WidgetOptions _widgetOptions;
        private readonly DashboardOptions _dashboardOptions;

        public DashboardRenderer(IOptions<WidgetOptions> widgetOptions, IOptions<DashboardOptions> dashboardOptions)
        {
            _widgetOptions = widgetOptions.Value;
            _dashboardOptions = dashboardOptions.Value;
        }

        public Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string dashboardName, object args = null)
        {
            throw new NotImplementedException();
        }
    }
}
