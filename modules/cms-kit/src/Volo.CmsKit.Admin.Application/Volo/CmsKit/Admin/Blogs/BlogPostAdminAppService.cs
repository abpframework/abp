using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
    public class BlogPostAdminAppService : CmsKitAppServiceBase, IBlogPostAdminAppService
    {
        protected BlogPostManager BlogPostManager { get; }
        protected IBlogPostRepository BlogPostRepository { get; }
        protected IBlogRepository BlogRepository { get; }
        protected ICmsUserLookupService UserLookupService { get; }

        public BlogPostAdminAppService(
            BlogPostManager blogPostManager,
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository,
            ICmsUserLookupService userLookupService)
        {
            BlogPostManager = blogPostManager;
            BlogPostRepository = blogPostRepository;
            BlogRepository = blogRepository;
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
                                                        input.Content,
                                                        input.CoverImageMediaId);

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
            blogPost.CoverImageMediaId = input.CoverImageMediaId;

            if (blogPost.Slug != input.Slug)
            {
                await BlogPostManager.SetSlugUrlAsync(blogPost, input.Slug);
            }

            await BlogPostRepository.UpdateAsync(blogPost);

            return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual async Task<BlogPostDto> GetAsync(Guid id)
        {
            var blogPost = await BlogPostRepository.GetAsync(id);

            return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual async Task<PagedResultDto<BlogPostListDto>> GetListAsync(BlogPostGetListInput input)
        {
            var blogs = (await BlogRepository.GetListAsync()).ToDictionary(x => x.Id);
            
            var blogPosts = await BlogPostRepository.GetListAsync(input.Filter, input.BlogId, input.MaxResultCount, input.SkipCount, input.Sorting);

            var count = await BlogPostRepository.GetCountAsync(input.Filter);

            var dtoList = blogPosts.Select(x => new BlogPostListDto {
                Id = x.Id,
                BlogId = x.BlogId,
                BlogName = blogs[x.BlogId].Name,
                Title = x.Title,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                Content = x.Content,
                CoverImageMediaId = x.CoverImageMediaId,
                CreationTime = x.CreationTime,
                LastModificationTime = x.LastModificationTime
            }).ToList();
            
            return new PagedResultDto<BlogPostListDto>(count, dtoList);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await BlogPostRepository.DeleteAsync(id);
        }
    }
}
