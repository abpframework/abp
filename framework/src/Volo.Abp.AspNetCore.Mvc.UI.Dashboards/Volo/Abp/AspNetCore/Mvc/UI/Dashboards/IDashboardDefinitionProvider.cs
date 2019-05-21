using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public interface IDashboardDefinitionProvider
    {
        List<DashboardDefinition> GetDefinitions();
    }
}
