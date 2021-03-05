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

        public bool IsEnabled { get; protected internal set; }

        public BlogFeature(Guid blogId, [NotNull] string featureName, bool isEnabled = true)
        {
            BlogId = blogId;
            FeatureName = Check.NotNullOrWhiteSpace(featureName, nameof(featureName));
            IsEnabled = isEnabled;
        }
        
        /*
         TODO: Overriding Equals is not a good practice for entities (see https://github.com/abpframework/abp/issues/1728)
               Also, the implementation is not a true equal implementation. It just special comparison for a specific case
               (used in BlogFeatureManager.GetListAsync). Remove Equals and just do the logic in-place, or create a static
               method like BlogFeature.IsSameBlogFeature(BlogFeature other).
        */
        public bool Equals(BlogFeature other)
        {
            return BlogId == other?.BlogId && FeatureName == other?.FeatureName;
        }
    }
}
