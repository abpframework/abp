using System.Collections.Generic;

namespace Volo.CmsKit.Polls;
public class WidgetContentFragment : ContentFragment
{
    public string Name { get; }

    public string ViewComponentName { get; set; }

    public IDictionary<string, object> Properties { get; set; }

    public WidgetContentFragment(string name)
    {
        Name = name;
        Properties = new Dictionary<string, object>();
    }
}
