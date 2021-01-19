using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public class BlogPostAdminAppService
        : CrudAppService<
            BlogPost,
            BlogPostDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateBlogPostDto>
        , IBlogPostAdminAppService
    {
        protected readonly IBlogPostManager BlogPostManager;
        protected readonly IBlogPostRepository BlogPostRepository;
        protected readonly IBlobContainer<BlogPostCoverImageContainer> BlobContainer;

        protected BlogPostAdminAppService(
            IRepository<BlogPost, Guid> repository,
            IBlogPostManager blogPostManager,
            IBlogPostRepository blogPostRepository) : base(repository)
        {
            BlogPostManager = blogPostManager;
            BlogPostRepository = blogPostRepository;


            GetListPolicyName = CmsKitAdminPermissions.BlogPosts.Default;
            GetPolicyName = CmsKitAdminPermissions.BlogPosts.Default;
            CreatePolicyName = CmsKitAdminPermissions.BlogPosts.Create;
            UpdatePolicyName = CmsKitAdminPermissions.BlogPosts.Update;
            DeletePolicyName = CmsKitAdminPermissions.BlogPosts.Delete;
        }

        public virtual async Task<BlogPostDto> GetByUrlSlugAsync(string urlSlug)
        {
            var blogPost = await BlogPostRepository.GetByUrlSlugAsync(urlSlug);

            return MapToGetOutputDto(blogPost);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
        public override async Task<BlogPostDto> CreateAsync(CreateUpdateBlogPostDto input)
        {
            var entity = await BlogPostManager
                                    .CreateAsync(
                                        new BlogPost(
                                            input.BlogId,
                                            input.Title,
                                            input.UrlSlug,
                                            input.CoverImageUrl));

            return MapToGetOutputDto(entity);
        }

        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
        public override async Task<BlogPostDto> UpdateAsync(Guid id, CreateUpdateBlogPostDto input)
        {
            var entity = await BlogPostRepository.GetAsync(id);

            entity.SetTitle(input.Title);
            entity.SetUrlSlug(input.UrlSlug);

            MapToEntity(input, entity);

            return MapToGetOutputDto(entity);
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
