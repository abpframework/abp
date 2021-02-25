using Microsoft.AspNetCore.Authorization;
using System;
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
    public class BlogPostAdminAppService
        : CrudAppService<
            BlogPost,
            BlogPostDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateBlogPostDto,
            UpdateBlogPostDto>
        , IBlogPostAdminAppService
    {
        protected IBlogPostManager BlogPostManager { get; }
        protected IBlogPostRepository BlogPostRepository { get; }
        protected IBlogRepository BlogRepository { get; }
        protected IBlobContainer<BlogPostCoverImageContainer> BlobContainer { get; }
        protected ICmsUserLookupService UserLookupService { get; }

        public BlogPostAdminAppService(
            IRepository<BlogPost, Guid> repository,
            IBlogPostManager blogPostManager,
            IBlogPostRepository blogPostRepository,
            IBlogRepository blogRepository,
            IBlobContainer<BlogPostCoverImageContainer> blobContainer,
            ICmsUserLookupService userLookupService) : base(repository)
        {
            BlogPostManager = blogPostManager;
            BlogPostRepository = blogPostRepository;
            BlogRepository = blogRepository;
            BlobContainer = blobContainer;
            UserLookupService = userLookupService;

            GetListPolicyName = CmsKitAdminPermissions.BlogPosts.Default;
            GetPolicyName = CmsKitAdminPermissions.BlogPosts.Default;
            CreatePolicyName = CmsKitAdminPermissions.BlogPosts.Create;
            UpdatePolicyName = CmsKitAdminPermissions.BlogPosts.Update;
            DeletePolicyName = CmsKitAdminPermissions.BlogPosts.Delete;
        }

        public virtual async Task<BlogPostDto> GetBySlugAsync(string blogSlug, string blogPostSlug)
        {
            var blog = await BlogRepository.GetBySlugAsync(blogSlug);

            var blogPost = await BlogPostRepository.GetBySlugAsync(blog.Id, blogPostSlug);

            return await MapToGetOutputDtoAsync(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
        public override async Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
        {
            _ = await UserLookupService.GetByIdAsync(CurrentUser.GetId());

            var entity = await BlogPostManager
                                    .CreateAsync(
                                        new BlogPost(
                                            GuidGenerator.Create(),
                                            input.BlogId,
                                            input.Title,
                                            input.Slug,
                                            input.ShortDescription));

            return await MapToGetOutputDtoAsync(entity);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
        public override async Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
        {
            var blogPost = await BlogPostRepository.GetAsync(id);

            blogPost.SetTitle(input.Title);

            if (blogPost.Slug != input.Slug)
            {
                await BlogPostManager.SetSlugUrlAsync(blogPost, input.Slug);
            }

            MapToEntity(input, blogPost);

            await BlogPostManager.UpdateAsync(blogPost);

            return await MapToGetOutputDtoAsync(blogPost);
        }

        public virtual async Task SetCoverImageAsync(Guid id, RemoteStreamContent streamContent)
        {
            await Repository.GetAsync(id);

            using (var stream = streamContent.GetStream())
            {
                await BlobContainer.SaveAsync(id.ToString(), stream, overrideExisting: true);
            }
        }

        public virtual async Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            var stream = await BlobContainer.GetAsync(id.ToString());

            return new RemoteStreamContent(stream);
        }
    }
}
