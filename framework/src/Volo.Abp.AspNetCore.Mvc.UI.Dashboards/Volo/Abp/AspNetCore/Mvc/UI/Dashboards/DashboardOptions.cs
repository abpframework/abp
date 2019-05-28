using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardOptions
    {
        public List<DashboardDefinition> Dashboards { get; }

        public DashboardOptions()
        {
            Dashboards = new List<DashboardDefinition>();
        }
    }
}
