using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostManager : DomainService, IBlogPostManager
    {
        protected IBlogPostRepository BlogPostRepository { get; }
        protected IBlogRepository BlogRepository { get; }

        public BlogPostManager(
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository)
        {
            BlogPostRepository = blogPostRepository;
            BlogRepository = blogRepository;
        }

        public virtual async Task<BlogPost> CreateAsync(
            [NotNull] CmsUser author, 
            [NotNull] Blog blog,
            [NotNull] string title,
            [NotNull] string slug,
            [CanBeNull] string shortDescription = null,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNull(blog, nameof(blog));

            await CheckBlogExistenceAsync(blog.Id);

            var blogPost = new BlogPost(
                        GuidGenerator.Create(),
                        blog.Id,
                        author.Id,
                        Check.NotNullOrEmpty(title, nameof(title)),
                        Check.NotNullOrEmpty(slug, nameof(slug)),
                        shortDescription
                        );

            await CheckSlugExistenceAsync(blog.Id, blogPost.Slug);

            return blogPost;
        }

        public virtual async Task UpdateAsync(BlogPost blogPost)
        {
            await CheckBlogExistenceAsync(blogPost.BlogId);

            await BlogPostRepository.UpdateAsync(blogPost);
        }

        public virtual async Task SetSlugUrlAsync(BlogPost blogPost, [NotNull] string newSlug)
        {
            Check.NotNullOrWhiteSpace(newSlug, nameof(newSlug));

            await CheckSlugExistenceAsync(blogPost.BlogId, newSlug);

            blogPost.SetSlug(newSlug);
        }

        private async Task CheckSlugExistenceAsync(Guid blogId, string slug)
        {
            if (await BlogPostRepository.SlugExistsAsync(blogId, slug))
            {
                throw new BlogPostSlugAlreadyExistException(blogId, slug);
            }
        }

        private async Task CheckBlogExistenceAsync(Guid blogId)
        {
            if (!await BlogRepository.ExistsAsync(blogId))
                throw new EntityNotFoundException(typeof(Blog), blogId);
        }
    }
}
