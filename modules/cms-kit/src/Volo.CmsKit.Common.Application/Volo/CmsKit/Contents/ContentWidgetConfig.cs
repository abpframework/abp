namespace Volo.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public string EditorComponentName { get; }

    public ContentWidgetConfig(string widgetName, string editorComponentName)
    {
        Name = widgetName;
        EditorComponentName = editorComponentName;
    }

}