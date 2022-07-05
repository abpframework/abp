namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public string InternalName { get; }

    public ContentWidgetConfig(string widgetName, string internalName)
    {
        Name = widgetName;
        InternalName = internalName;
    }

}