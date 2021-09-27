using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

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
        public virtual Task<BlogPostPublicDto> GetAsync(string blogSlug, string blogPostSlug)
        {
            return BlogPostPublicAppService.GetAsync(blogSlug, blogPostSlug);
        }

        [HttpGet]
        [Route("{blogSlug}")]
        public virtual Task<PagedResultDto<BlogPostPublicDto>> GetListAsync(string blogSlug, PagedAndSortedResultRequestDto input)
        {
            return BlogPostPublicAppService.GetListAsync(blogSlug, input);
        }
    }
}
