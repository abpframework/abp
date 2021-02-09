using JetBrains.Annotations;
using System;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Blogs.Extensions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedAggregateRootWithUser<Guid, CmsUser>, IMultiTenant
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string Slug { get; protected set; }

        public string ShortDescription { get; protected set; }

        public Guid? TenantId { get; protected set; }

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

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), BlogPostConsts.MaxTitleLength);
        }

        internal void SetSlug(string slug)
        {
            Check.NotNullOrWhiteSpace(slug, nameof(slug), BlogPostConsts.MaxSlugLength, BlogPostConsts.MinSlugLength);

            Slug = slug.NormalizeSlug();
        }

        public void SetShortDescription(string shortDescription)
        {
            ShortDescription = Check.Length(shortDescription, nameof(shortDescription), BlogPostConsts.MaxShortDescriptionLength);
        }
    }
}
