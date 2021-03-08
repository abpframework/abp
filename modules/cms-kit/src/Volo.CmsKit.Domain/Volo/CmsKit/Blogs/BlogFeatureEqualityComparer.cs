using System.Collections.Generic;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureEqualityComparer : IEqualityComparer<BlogFeature>
    {
        public bool Equals(BlogFeature x, BlogFeature y)
        {
            return x?.BlogId == y?.BlogId && x?.FeatureName == y?.FeatureName;
        }

        public int GetHashCode(BlogFeature obj)
        {
            return obj.GetHashCode();
        }
    }
}
