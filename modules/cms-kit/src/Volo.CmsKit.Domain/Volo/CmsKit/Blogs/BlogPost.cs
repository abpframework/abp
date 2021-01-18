using JetBrains.Annotations;
using System;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedAggregateRootWithUser<Guid, CmsUser> , IMultiTenant
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string ShortDescription { get; set; }

        public string UrlSlug { get; protected set; }

        public string CoverImageUrl { get; set; }

        public Guid? TenantId { get; }

        protected BlogPost()
        {
        }

        public BlogPost(
            Guid blogId,
            [NotNull] string title,
            [NotNull] string urlSlug,
            [CanBeNull] string shortDescription = null,
            [CanBeNull] string coverImageUrl = null,
            bool isPublished = true)
        {
            BlogId = blogId;
            SetTitle(title);
            SetUrlSlug(urlSlug);
            ShortDescription = shortDescription;
            CoverImageUrl = coverImageUrl;
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), BlogPostConsts.MaxTitleLength);
        }

        public void SetUrlSlug(string urlSlug)
        {
            Check.NotNullOrWhiteSpace(urlSlug, nameof(urlSlug), BlogPostConsts.MaxUrlSlugLength, BlogPostConsts.MinUrlSlugLength);

            UrlSlug = NormalizeUrlSlug(urlSlug);
        }

        private string NormalizeUrlSlug(string value)
        {
            value = value.ToLowerInvariant();

            // TODO: Find best way to unidecode.
            // value = value.Unidecode(); 

            // Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            // Trim dashes from end & dots
            value = value.Trim('-', '_', '.');

            // Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
