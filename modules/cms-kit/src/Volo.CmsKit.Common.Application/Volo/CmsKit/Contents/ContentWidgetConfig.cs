using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    //TODO remove - just for example
    public List<string> Properties { get; } = new() { "Type", "Code" };

    public ContentWidgetConfig(string widgetName)
    {
        Name = widgetName;
    }
}