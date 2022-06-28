using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidget(string widgetName, string widgetKey, List<PropertyDto> properties = null)
    {
        var conf = new ContentWidgetConfig(widgetKey);
        conf.Properties.AddRange(properties);
        WidgetConfigs.Add(widgetName, conf);
    }
}