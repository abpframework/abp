using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureCacheKey
    {
        public BlogFeatureCacheKey(Guid id, [NotNull] string featureName)
        {
            Id = id;
            FeatureName = Check.NotNullOrEmpty(featureName, nameof(featureName));
        }

        public Guid Id { get; set; }

        public string FeatureName { get; set; }

        public override string ToString()
        {
            return $"BlogFeature_{Id}_{FeatureName}";
        }
    }
}
