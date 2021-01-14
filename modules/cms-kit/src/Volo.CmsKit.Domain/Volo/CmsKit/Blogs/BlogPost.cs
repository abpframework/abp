using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedEntity<Guid>
    {
        public Guid BlogId { get; protected set; }

        public string Title { get; protected set; }

        public string UrlSlug { get; protected set; }

        public string CoverImageUrl { get; set; }

        public bool IsPublished { get; protected set; }

        public DateTime? PublishDate { get; protected set; }

        protected BlogPost()
        {
        }

        public BlogPost(
            Guid blogId,
            [NotNull] string title,
            [NotNull] string urlSlug,
            [CanBeNull] string coverImageUrl = null,
            bool isPublished = true)
        {
            BlogId = blogId;
            SetTitle(title);
            SetUrlSlug(urlSlug);
            CoverImageUrl = coverImageUrl;
            IsPublished = isPublished;
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), BlogPostConsts.MaxTitleLength);
        }

        public void SetUrlSlug(string urlSlug)
        {
            UrlSlug = Check.NotNullOrWhiteSpace(urlSlug, nameof(urlSlug), BlogPostConsts.MaxUrlSlugLength, BlogPostConsts.MinUrlSlugLength);
        }

        public void SetIsPublished(bool isPublished)
        {
            if (!IsPublished && isPublished)
            {
                this.PublishDate = DateTime.UtcNow;
            }

            IsPublished = IsPublished;
        }
    }
}
