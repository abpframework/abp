namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class AbpWidgetOptions
    {
        public WidgetDefinitionCollection Widgets { get; }

        public AbpWidgetOptions()
        {
            Widgets = new WidgetDefinitionCollection();
        }
    }
}
