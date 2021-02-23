using JetBrains.Annotations;
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

        protected IBlogPostRepository BlogPostRepository { get; }

        protected IBlobContainer<BlogPostCoverImageContainer> BlobContainer { get; }

        public BlogPostPublicAppService(
            IBlogRepository blogRepository,
            IBlogPostRepository blogPostRepository,
            IBlobContainer<BlogPostCoverImageContainer> blobContainer)
        {
            BlogRepository = blogRepository;
            BlogPostRepository = blogPostRepository;
            BlobContainer = blobContainer;
        }

        public virtual async Task<BlogPostPublicDto> GetAsync([NotNull] string blogSlug, [NotNull] string blogPostSlug)
        {
            var blog = await BlogRepository.GetBySlugAsync(blogSlug);

            var blogPost = await BlogPostRepository.GetBySlugAsync(blog.Id, blogPostSlug);

            return ObjectMapper.Map<BlogPost, BlogPostPublicDto>(blogPost);
        }

        public virtual async Task<PagedResultDto<BlogPostPublicDto>> GetListAsync([NotNull] string blogSlug, PagedAndSortedResultRequestDto input)
        {
            var blog = await BlogRepository.GetBySlugAsync(blogSlug);

            var blogPosts = await BlogPostRepository.GetPagedListAsync(blog.Id, input.SkipCount, input.MaxResultCount, input.Sorting);

            return new PagedResultDto<BlogPostPublicDto>(
                await BlogPostRepository.GetCountAsync(blog.Id),
                ObjectMapper.Map<List<BlogPost>, List<BlogPostPublicDto>>(blogPosts));
        }

        public virtual async Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            var stream = await BlobContainer.GetAsync(id.ToString());

            return new RemoteStreamContent(stream);
        }
    }
}
