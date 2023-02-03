using System.Collections.Generic;

namespace Volo.CmsKit.Web.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidget(string widgetType, string widgetName, string parameterWidgetName = null)
    {
        var config = new ContentWidgetConfig(widgetName, parameterWidgetName);
        WidgetConfigs.Add(widgetType, config);
    }
}