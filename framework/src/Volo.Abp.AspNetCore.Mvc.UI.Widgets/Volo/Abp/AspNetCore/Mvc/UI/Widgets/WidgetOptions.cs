using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetOptions
    {
        public List<WidgetDefinition> Widgets { get; }

        public WidgetOptions()
        {
            Widgets = new List<WidgetDefinition>();
        }
    }
}
