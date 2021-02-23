using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager : DomainService, IBlogManager
    {
        protected IBlogRepository BlogRepository { get; }
        
        public BlogManager(IBlogRepository blogRepository)
        {
            BlogRepository = blogRepository;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var hasPosts = await BlogRepository.HasPostsAsync(id, cancellationToken);

            if (hasPosts)
            {
                throw new BlogCannotBeDeletedException(id);
            }

            await BlogRepository.DeleteAsync(id, cancellationToken: cancellationToken);
        }
    }
}