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

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
        public virtual Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
        {
            return BlogPostAdminAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return BlogPostAdminAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual Task<BlogPostDto> GetAsync(Guid id)
        {
            return BlogPostAdminAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("{blogSlug}/{blogPostSlug}")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual Task<BlogPostDto> GetBySlugAsync(string blogSlug, string blogPostSlug)
        {
            return BlogPostAdminAppService.GetBySlugAsync(blogSlug, blogPostSlug);
        }

        [HttpGet]
        [Route("{id}/cover-image")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            Response.Headers.Add("Content-Disposition", $"inline;filename=\"{id}\"");
            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.Headers.Add("Cache-Control", "max-age=120");
            Response.ContentType = "image";

            return BlogPostAdminAppService.GetCoverImageAsync(id);
        }

        [HttpGet]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual Task<PagedResultDto<BlogPostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return BlogPostAdminAppService.GetListAsync(input);
        }

        [NonAction]
        public virtual Task SetCoverImageAsync(Guid id, RemoteStreamContent streamContent)
        {
            return BlogPostAdminAppService.SetCoverImageAsync(id, streamContent);
        }

        [HttpPost]
        [Route("{id}/cover-image")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
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

            return CreatedAtAction(nameof(GetCoverImageAsync), new { id });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
        public virtual Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
        {
            return BlogPostAdminAppService.UpdateAsync(id, input);
        }
    }
}
