using System;

namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public Type ViewComponentType { get; set; }

    public ContentWidgetConfig(string name)
    {
        Name = name;
    }
}