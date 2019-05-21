namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public static class WidgetOptionsExtensions
    {
        public static void AddWidgets<T>(this WidgetOptions options)
            where T : IWidgetDefinitionProvider, new()
        {
            var widgets = new T().GetDefinitions();
            options.Widgets.AddRange(widgets);
        }
    }
}
