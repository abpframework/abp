using System;

namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public Type ViewComponentType { get; set; } //TODO: Remove this

    public ContentWidgetConfig(string name) //TODO: widgetName
    {
        Name = name;
    }
}