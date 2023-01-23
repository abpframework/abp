using System;

namespace Volo.CmsKit.Pages;

public class PageCacheItem
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }
}