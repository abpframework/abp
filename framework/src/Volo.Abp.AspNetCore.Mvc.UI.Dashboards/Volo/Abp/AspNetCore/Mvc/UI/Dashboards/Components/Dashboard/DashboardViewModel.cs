using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards.Components.Dashboard
{
    public class DashboardViewModel
    {
        public DashboardDefinition Dashboard { get; set; }

        public List<WidgetDefinition> Widgets { get; set; }

        public DashboardViewModel(DashboardDefinition dashboard, List<WidgetDefinition> widgets)
        {
            Dashboard = dashboard;
            Widgets = widgets;
        }

        public WidgetDefinition GetWidget(string name)
        {
            return Widgets.Single(d => d.Name.Equals(name));
        }

        public async Task<bool> CheckPermissionsAsync(IAuthorizationService authorizationService, WidgetDefinition widget)
        {
            foreach (var permission in widget.RequiredPermissions)
            {
                if (!await authorizationService.IsGrantedAsync(permission))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
