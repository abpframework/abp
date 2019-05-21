namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public static class WidgetOptionsExtensions
    {
        public static void AddDashboards<T>(this DashboardOptions options)
            where T : IDashboardDefinitionProvider, new()
        {
            var widgets = new T().GetDefinitions();
            options.Dashboards.AddRange(widgets);
        }
    }
}
