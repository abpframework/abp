using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeature : FullAuditedAggregateRoot<Guid>
    {
        protected BlogFeature() // Keep for ORM
        {
        }

        public BlogFeature(Guid blogId, [NotNull] string featureName, bool enabled = true)
        {
            BlogId = blogId;
            FeatureName = Check.NotNullOrWhiteSpace(featureName, nameof(featureName));
            Enabled = enabled;
        }

        public Guid BlogId { get; protected set; }

        public string FeatureName { get; protected set; }

        public bool Enabled { get; set; } = true;
    }
}
