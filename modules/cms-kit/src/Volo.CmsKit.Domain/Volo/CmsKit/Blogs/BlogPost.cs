using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Blogs.Extensions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedAggregateRootWithUser<Guid, CmsUser>, IMultiTenant
    {
        public virtual Guid BlogId { get; protected set; }

        [NotNull] 
        public virtual string Title { get; protected set; }

        [NotNull] 
        public virtual string Slug { get; protected set; }

        [NotNull] 
        public virtual string ShortDescription { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        protected BlogPost()
        {
        }

        public BlogPost(
            Guid id,
            Guid blogId,
            [NotNull] string title,
            [NotNull] string slug,
            [CanBeNull] string shortDescription = null) : base(id)
        {
            BlogId = blogId;
            SetTitle(title);
            SetSlug(slug);
            ShortDescription = shortDescription;
        }

        public virtual void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), BlogPostConsts.MaxTitleLength);
        }

        internal void SetSlug(string slug)
        {
            Check.NotNullOrWhiteSpace(slug, nameof(slug), BlogPostConsts.MaxSlugLength, BlogPostConsts.MinSlugLength);

            Slug = slug.NormalizeSlug();
        }

        public virtual void SetShortDescription(string shortDescription)
        {
            ShortDescription = Check.Length(shortDescription, nameof(shortDescription), BlogPostConsts.MaxShortDescriptionLength);
        }
    }
}
