using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager : DomainService, IBlogManager
    {
        protected readonly IBlogRepository BlogRepository;
        
        public BlogManager(IBlogRepository blogRepository)
        {
            BlogRepository = blogRepository;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var hasPosts = await BlogRepository.HasPostsAsync(id);

            if (hasPosts)
            {
                throw new BlogHasPostsCannotBeDeletedException(id);
            }

            await BlogRepository.DeleteAsync(id);
        }
    }
}