using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public class BlogPostAdminAppService: CmsKitAppServiceBase, IBlogPostAdminAppService
    {
        protected BlogPostManager BlogPostManager { get; }
        protected IBlogPostRepository BlogPostRepository { get; }
        protected IBlogRepository BlogRepository { get; }
        protected IBlobContainer<BlogPostCoverImageContainer> BlobContainer { get; }
        protected ICmsUserLookupService UserLookupService { get; }

        public BlogPostAdminAppService(
            BlogPostManager blogPostManager,
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository,
            IBlobContainer<BlogPostCoverImageContainer> blobContainer,
            ICmsUserLookupService userLookupService)
        {
            BlogPostManager = blogPostManager;
            BlogPostRepository = blogPostRepository;
            BlogRepository = blogRepository;
            BlobContainer = blobContainer;
            UserLookupService = userLookupService;

        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
        public virtual async Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
        {
            var author = await UserLookupService.GetByIdAsync(CurrentUser.GetId());

            var blog = await BlogRepository.GetAsync(input.BlogId);

            var blogPost = await BlogPostManager.CreateAsync(
                                                        author,
                                                        blog,
                                                        input.Title,
                                                        input.Slug,
                                                        input.ShortDescription,
                                                        input.Content);

            await BlogPostRepository.InsertAsync(blogPost);

            return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
        public virtual async Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
        {
            var blogPost = await BlogPostRepository.GetAsync(id);

            blogPost.SetTitle(input.Title);
            blogPost.SetShortDescription(input.ShortDescription);
            blogPost.SetContent(input.Content);
            
            if (blogPost.Slug != input.Slug)
            {
                await BlogPostManager.SetSlugUrlAsync(blogPost, input.Slug);
            }

            await BlogPostRepository.UpdateAsync(blogPost);

            return ObjectMapper.Map<BlogPost,BlogPostDto>(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
        public virtual async Task SetCoverImageAsync(Guid id, RemoteStreamContent streamContent)
        {
            await BlogPostRepository.GetAsync(id);

            using (var stream = streamContent.GetStream())
            {
                await BlobContainer.SaveAsync(id.ToString(), stream, overrideExisting: true);
            }
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual async Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            var stream = await BlobContainer.GetAsync(id.ToString());

            return new RemoteStreamContent(stream);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual async Task<BlogPostDto> GetAsync(Guid id)
        {
            var blogPost = await BlogPostRepository.GetAsync(id);

            return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual async Task<PagedResultDto<BlogPostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty())
            {
                input.Sorting = nameof(BlogPost.CreationTime) + " desc";
            }

            var blogPosts = await BlogPostRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, includeDetails: true);

            var count = await BlogPostRepository.GetCountAsync();

            return new PagedResultDto<BlogPostDto>(
                count,
                ObjectMapper.Map<List<BlogPost>, List<BlogPostDto>>(blogPosts));
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await BlogPostRepository.DeleteAsync(id);
        }
    }
}
