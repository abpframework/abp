using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets.Components;

public class WidgetResourcesViewModel
{
    public IReadOnlyList<WidgetDefinition> Widgets { get; set; }
}
