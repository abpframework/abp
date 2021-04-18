using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostManager : DomainService
    {
        protected IBlogPostRepository BlogPostRepository { get; }
        protected IBlogRepository BlogRepository { get; }
        protected IDefaultBlogFeatureProvider BlogFeatureProvider { get; }


        public BlogPostManager(
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository,
            IDefaultBlogFeatureProvider blogFeatureProvider)
        {
            BlogPostRepository = blogPostRepository;
            BlogRepository = blogRepository;
            BlogFeatureProvider = blogFeatureProvider;
        }

        public virtual async Task<BlogPost> CreateAsync(
            [NotNull] CmsUser author, 
            [NotNull] Blog blog,
            [NotNull] string title,
            [NotNull] string slug,
            [CanBeNull] string shortDescription = null,
            [CanBeNull] string content = null,
            [CanBeNull] Guid? coverImageMediaId = null)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNull(blog, nameof(blog));
            Check.NotNullOrEmpty(title, nameof(title));
            Check.NotNullOrEmpty(slug, nameof(slug));

            var blogPost = new BlogPost(
                        GuidGenerator.Create(),
                        blog.Id,
                        author.Id,
                        title,
                        slug,
                        shortDescription,
                        content,
                        coverImageMediaId,
                        CurrentTenant.Id
                        );

            await CheckSlugExistenceAsync(blog.Id, blogPost.Slug);

            return blogPost;
        }

        public virtual async Task SetSlugUrlAsync(BlogPost blogPost, [NotNull] string newSlug)
        {
            Check.NotNullOrWhiteSpace(newSlug, nameof(newSlug));

            await CheckSlugExistenceAsync(blogPost.BlogId, newSlug);

            blogPost.SetSlug(newSlug);
        }

        protected virtual async Task CheckSlugExistenceAsync(Guid blogId, string slug)
        {
            if (await BlogPostRepository.SlugExistsAsync(blogId, slug))
            {
                throw new BlogPostSlugAlreadyExistException(blogId, slug);
            }
        }

        protected virtual async Task CheckBlogExistenceAsync(Guid blogId)
        {
            if (!await BlogRepository.ExistsAsync(blogId))
            {
                throw new EntityNotFoundException(typeof(Blog), blogId);
            }
        }
    }
}
