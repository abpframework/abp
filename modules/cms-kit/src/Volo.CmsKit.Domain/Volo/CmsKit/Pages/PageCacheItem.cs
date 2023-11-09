using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Pages;

public class PageCacheItem : ExtensibleObject
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }

    public static string GetKey(string slug)
    {
        return $"CmsPage_{slug}";
    }
}