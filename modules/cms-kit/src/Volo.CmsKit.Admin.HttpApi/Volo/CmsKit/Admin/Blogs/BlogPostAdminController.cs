using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    [Route("api/cms-kit-admin/blogs/blog-posts")]
    public class BlogPostAdminController : CmsKitAdminController, IBlogPostAdminAppService
    {
        protected readonly IBlogPostAdminAppService BlogPostAdminAppService;

        public BlogPostAdminController(IBlogPostAdminAppService blogPostAdminAppService)
        {
            BlogPostAdminAppService = blogPostAdminAppService;
        }

        public virtual Task<BlogPostDto> CreateAsync(CreateUpdateBlogPostDto input)
        {
            return BlogPostAdminAppService.CreateAsync(input);
        }

        public virtual Task DeleteAsync(Guid id)
        {
            return BlogPostAdminAppService.DeleteAsync(id);
        }

        public virtual Task<BlogPostDto> GetAsync(Guid id)
        {
            return BlogPostAdminAppService.GetAsync(id);
        }

        public virtual Task<BlogPostDto> GetByUrlSlugAsync(string urlSlug)
        {
            return BlogPostAdminAppService.GetByUrlSlugAsync(urlSlug);
        }

        public virtual Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            Response.Headers.Add("Content-Disposition", $"inline;filename=\"{id}\"");
            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.Headers.Add("Cache-Control", "max-age=120");
            Response.ContentType = "image/*";

            return BlogPostAdminAppService.GetCoverImageAsync(id);
        }

        public virtual Task<PagedResultDto<BlogPostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return BlogPostAdminAppService.GetListAsync(input);
        }

        public virtual Task SetCoverImageAsync(Guid id, RemoteStreamContent streamContent)
        {
            return BlogPostAdminAppService.SetCoverImageAsync(id, streamContent);
        }

        public virtual async Task<IActionResult> UploadCoverImageAsync(Guid id, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            using (var stream = file.OpenReadStream())
            {
                await BlogPostAdminAppService.SetCoverImageAsync(id, new RemoteStreamContent(stream));
            }

            return StatusCode(201);
        }

        public virtual Task<BlogPostDto> UpdateAsync(Guid id, CreateUpdateBlogPostDto input)
        {
            return BlogPostAdminAppService.UpdateAsync(id, input);
        }
    }
}
