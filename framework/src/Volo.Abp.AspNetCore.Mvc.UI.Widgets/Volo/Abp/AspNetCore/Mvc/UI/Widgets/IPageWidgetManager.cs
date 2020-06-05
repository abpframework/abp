using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public interface IPageWidgetManager
    {
        bool TryAdd(WidgetDefinition widget);

        IReadOnlyList<WidgetDefinition> GetAll();
    }
}