using System;

namespace Volo.CmsKit.Blogs;

[Serializable]
public class BlogFeatureCacheItem
{
    public Guid Id { get; set; }
    public string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
}
