using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class GlobalFilterOptions
    {
        public List<GlobalFilterDefinition> GlobalFilters { get; }

        public GlobalFilterOptions()
        {
            GlobalFilters = new List<GlobalFilterDefinition>();
        }
    }
}
