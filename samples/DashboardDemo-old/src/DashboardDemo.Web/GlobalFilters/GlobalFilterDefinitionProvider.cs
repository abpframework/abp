using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.Localization;

namespace DashboardDemo.GlobalFilters
{
    public static class GlobalFilterDefinitionProvider
    {
        public static List<GlobalFilterDefinition> GetDefinitions()
        {
            var dateRangeFilter = new GlobalFilterDefinition(
                DateRangeGlobalFilterViewComponent.Name,
                    LocalizableString.Create<DashboardDemoResource>("DateRangeFilter"),
                typeof(DateRangeGlobalFilterViewComponent)
                );

            return new List<GlobalFilterDefinition>
            {
                dateRangeFilter
            };
        }
    }
}
