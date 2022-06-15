using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidgetConfig(string name, ContentWidgetConfig contentWidgetConfig)
    {
        WidgetConfigs.Add(name, contentWidgetConfig);
    }
}