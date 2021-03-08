﻿using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.Blogs.Extensions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPost : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid BlogId { get; protected set; }

        [NotNull] 
        public virtual string Title { get; protected set; }

        [NotNull] 
        public virtual string Slug { get; protected set; }

        [NotNull] 
        public virtual string ShortDescription { get; protected set; }

        public virtual string Content { get; protected set; }
        
        public virtual Guid? TenantId { get; protected set; }

        public Guid AuthorId { get; set; }

        public virtual CmsUser Author { get; set; }

        protected BlogPost()
        {
        }

        internal BlogPost(
            Guid id,
            Guid blogId,
            Guid authorId,
            [NotNull] string title,
            [NotNull] string slug,
            [CanBeNull] string shortDescription = null,
            [CanBeNull] string content = null,
            [CanBeNull] Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            BlogId = blogId;
            AuthorId = authorId;
            SetTitle(title);
            SetSlug(slug);
            SetShortDescription(shortDescription);
            SetContent(content);
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

        public virtual void SetContent(string content)
        {
            Content = Check.Length(content, nameof(content), BlogPostConsts.MaxContentLength);
        }
    }
}
