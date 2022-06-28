using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public List<PropertyDto> Properties { get; }

    public ContentWidgetConfig(string widgetName)
    {
        Properties = new();
        Name = widgetName;
    }

    public void AddProperty(string key, string name)
    {
        Properties.Add(new PropertyDto() { Key = key, Name = name });
    }

}