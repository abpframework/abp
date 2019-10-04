using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardGlobalFilterConfiguration
    {
        [NotNull]
        public string GlobalFilterName { get; }

        public DashboardGlobalFilterConfiguration([NotNull] string globalFilterName)
        {
            GlobalFilterName = Check.NotNullOrWhiteSpace(globalFilterName, nameof(globalFilterName));
        }
    }
}