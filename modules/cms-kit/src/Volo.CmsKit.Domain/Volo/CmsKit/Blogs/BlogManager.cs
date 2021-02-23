using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager : DomainService, IBlogManager
    {
        protected readonly IBlogPostRepository BlogPostRepository;

        public BlogManager(IBlogPostRepository blogPostRepository)
        {
            BlogPostRepository = blogPostRepository;
        }

        public virtual async Task CheckDeleteAsync(Guid id)
        {
            var hasPosts = await BlogPostRepository.ExistsAsync(id);

            if (hasPosts)
            {
                throw new BlogHasPostsCannotBeDeletedException(id);
            }
        }
    }
}