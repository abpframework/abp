using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidget(string widgetName, string widgetKey)
    {
        WidgetConfigs.Add(widgetName, new ContentWidgetConfig(widgetKey));
    }
}