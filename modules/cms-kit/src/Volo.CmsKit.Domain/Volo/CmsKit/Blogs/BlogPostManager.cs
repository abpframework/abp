using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostManager : IBlogPostManager
    {
        protected readonly IBlogPostRepository _blogPostRepository;

        public BlogPostManager(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await CheckUrlSlugExistenceAsync(blogPost.UrlSlug);

            return await _blogPostRepository.InsertAsync(blogPost);
        }

        public async Task UpdateAsync(BlogPost blogPost)
        {
            await CheckUrlSlugExistenceAsync(blogPost.UrlSlug);

            await _blogPostRepository.UpdateAsync(blogPost);
        }

        private async Task CheckUrlSlugExistenceAsync(string urlSlug)
        {
            if (await _blogPostRepository.SlugExistsAsync(urlSlug))
            {
                throw new BlogPostUrlSlugAlreadyExistException(urlSlug);
            }
        }
    }
}
