using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Public.Blogs
{
    public class BlogPostPublicAppService : CmsKitPublicAppServiceBase, IBlogPostPublicAppService
    {
        protected IBlogRepository BlogRepository { get; }

        protected IBlobContainer<BlogPostCoverImageContainer> BlobContainer { get; }

        protected IBlogPostRepository BlogPostRepository { get; }

        public BlogPostPublicAppService(
            IBlogRepository blogRepository,
            IBlogPostRepository blogPostRepository)
        {
            BlogRepository = blogRepository;
            BlogPostRepository = blogPostRepository;
        }

        public async Task<BlogPostPublicDto> GetAsync(string blogUrlSlug, string blogPostUrlSlug)
        {
            var blog = await BlogRepository.GetByUrlSlugAsync(blogUrlSlug);

            var blogPost = await BlogPostRepository.GetByUrlSlugAsync(blog.Id, blogPostUrlSlug);

            return ObjectMapper.Map<BlogPost, BlogPostPublicDto>(blogPost);
        }

        public async Task<PagedResultDto<BlogPostPublicDto>> GetListAsync(string blogUrlSlug, PagedAndSortedResultRequestDto input)
        {
            var blog = await BlogRepository.GetByUrlSlugAsync(blogUrlSlug);

            var blogPosts = await BlogPostRepository.GetPagedListAsync(blog.Id, input.SkipCount, input.MaxResultCount, input.Sorting);

            return new PagedResultDto<BlogPostPublicDto>(
                await BlogPostRepository.GetCountAsync(blog.Id),
                ObjectMapper.Map<List<BlogPost>, List<BlogPostPublicDto>>(blogPosts));
        }

        public async Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            var stream = await BlobContainer.GetAsync(id.ToString());

            return new RemoteStreamContent(stream);
        }
    }
}
