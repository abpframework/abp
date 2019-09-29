namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetOptions
    {
        public WidgetDefinitionCollection Widgets { get; }

        public WidgetOptions()
        {
            Widgets = new WidgetDefinitionCollection();
        }
    }
}
