using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Blogs;

[Serializable]
public class BlogFeatureCacheItem : ExtensibleObject
{
    public Guid Id { get; set; }
    public string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
}
