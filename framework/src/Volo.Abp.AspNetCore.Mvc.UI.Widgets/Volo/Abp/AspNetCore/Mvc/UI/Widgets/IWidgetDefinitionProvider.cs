using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public interface IWidgetDefinitionProvider
    {
        List<WidgetDefinition> GetDefinitions();
    }
}
