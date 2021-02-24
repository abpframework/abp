using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager : DomainService
    {
        protected IBlogRepository BlogRepository { get; }
        
        public BlogManager(IBlogRepository blogRepository)
        {
            BlogRepository = blogRepository;
        }

        public virtual async Task<Blog> CreateAsync([NotNull] string name, [NotNull] string slug)
        {
            await CheckSlugAsync(slug);

            return new Blog(GuidGenerator.Create(), name, slug, CurrentTenant.Id);
        }

        public virtual async Task<Blog> UpdateAsync([NotNull] Blog blog, [NotNull] string name, [NotNull] string slug)
        {
            if (slug != blog.Slug)
            {
                await CheckSlugAsync(slug);
            }
            
            blog.SetName(name);
            blog.SetSlug(slug);

            return blog;
        }

        protected virtual async Task CheckSlugAsync([NotNull] string slug)
        {
            if (await BlogRepository.SlugExistsAsync(slug))
            {
                throw new BlogSlugAlreadyExistException(slug);
            }
        }
    }
}