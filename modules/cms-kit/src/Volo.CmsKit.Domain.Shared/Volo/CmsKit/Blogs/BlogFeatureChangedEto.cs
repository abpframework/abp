using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
