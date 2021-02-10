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
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/blog-posts")]
    public class BlogPostPublicController : CmsKitPublicControllerBase, IBlogPostPublicAppService
    {
        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        public BlogPostPublicController(IBlogPostPublicAppService blogPostPublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
        }

        [HttpGet]
        [Route("{blogSlug}/{blogPostSlug}")]
        public Task<BlogPostPublicDto> GetAsync(string blogSlug, string blogPostSlug)
        {
            return BlogPostPublicAppService.GetAsync(blogSlug, blogPostSlug);
        }

        [HttpGet]
        [Route("{id}/cover-image")]
        public Task<RemoteStreamContent> GetCoverImageAsync(Guid id)
        {
            Response.Headers.Add("Content-Disposition", $"inline;filename=\"{id}\"");
            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.Headers.Add("Cache-Control", "max-age=120");
            Response.ContentType = "image";

            return BlogPostPublicAppService.GetCoverImageAsync(id);
        }

        [HttpGet]
        [Route("{blogSlug}")]
        public Task<PagedResultDto<BlogPostPublicDto>> GetListAsync(string blogSlug, PagedAndSortedResultRequestDto input)
        {
            return BlogPostPublicAppService.GetListAsync(blogSlug, input);
        }
    }
}
