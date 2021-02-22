using System;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Blogs
{
    [EventName("CmsKit.Blogs.BlogFeature.Changed")]
    public class BlogFeatureChangedEto
    {
        public Guid BlogId { get; set; }
        public string FeatureName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
