using System;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

public class ContentWidgetConfig
{
    public string Name { get; }
    public Type ViewComponentType { get; set; }
    
    public ContentWidgetConfig(string name)
    {
        Name = name;
    }
}