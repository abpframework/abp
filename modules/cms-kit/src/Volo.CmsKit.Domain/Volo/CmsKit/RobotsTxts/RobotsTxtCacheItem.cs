using System;

namespace Volo.CmsKit.RobotsTxts;

[Serializable]
public class RobotsTxtCacheItem
{
    public const string CacheKey = "CmsKit.RobotsTxt";

    /// <summary>
    /// The content stored for the tenant (or host). Null if nothing is stored.
    /// </summary>
    public string Content { get; set; }

    public RobotsTxtCacheItem()
    {
    }

    public RobotsTxtCacheItem(string content)
    {
        Content = content;
    }
}
