using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeature : FullAuditedAggregateRoot<Guid>, IEquatable<BlogFeature>
    {
        public Guid BlogId { get; protected set; }

        public string FeatureName { get; protected set; }

        public bool IsEnabled { get; set; } = true;

        public BlogFeature(Guid blogId, [NotNull] string featureName, bool isEnabled = true)
        {
            BlogId = blogId;
            FeatureName = Check.NotNullOrWhiteSpace(featureName, nameof(featureName));
            IsEnabled = isEnabled;
        }
        
        public bool Equals(BlogFeature other)
        {
            return BlogId == other?.BlogId && FeatureName == other?.FeatureName;
        }
    }
}
