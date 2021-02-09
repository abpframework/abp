using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostManager : DomainService, IBlogPostManager
    {
        protected readonly IBlogPostRepository blogPostRepository;
        protected readonly IBlogRepository blogRepository;

        public BlogPostManager(
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogRepository = blogRepository;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await CheckBlogExistenceAsync(blogPost.BlogId);

            await CheckSlugExistenceAsync(blogPost.BlogId, blogPost.Slug);

            return await blogPostRepository.InsertAsync(blogPost);
        }

        public async Task UpdateAsync(BlogPost blogPost)
        {
            await CheckBlogExistenceAsync(blogPost.BlogId);

            await blogPostRepository.UpdateAsync(blogPost);
        }

        public async Task SetSlugUrlAsync(BlogPost blogPost, string newSlug)
        {
            await CheckSlugExistenceAsync(blogPost.BlogId, newSlug);

            blogPost.SetSlug(newSlug);
        }

        private async Task CheckSlugExistenceAsync(Guid blogId, string slug)
        {
            if (await blogPostRepository.SlugExistsAsync(blogId, slug))
            {
                throw new BlogPostSlugAlreadyExistException(blogId, slug);
            }
        }

        private async Task CheckBlogExistenceAsync(Guid blogId)
        {
            if (!await blogRepository.ExistsAsync(blogId))
                throw new EntityNotFoundException(typeof(Blog), blogId);
        }
    }
}
