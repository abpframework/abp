using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Blogs;

public class BlogFeature : FullAuditedAggregateRoot<Guid>
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
}
