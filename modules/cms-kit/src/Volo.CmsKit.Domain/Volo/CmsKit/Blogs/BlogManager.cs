using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager : DomainService, IBlogManager
    {
        protected readonly IBlogRepository BlogRepository;
        protected readonly IBlogPostRepository BlogPostRepository;
        
        public BlogManager(IBlogRepository blogRepository, IBlogPostRepository blogPostRepository)
        {
            BlogRepository = blogRepository;
            BlogPostRepository = blogPostRepository;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var hasPosts = await BlogPostRepository.ExistsAsync(id);

            if (hasPosts)
            {
                throw new BlogHasPostsCannotBeDeletedException(id);
            }

            await BlogRepository.DeleteAsync(id);
        }
    }
}