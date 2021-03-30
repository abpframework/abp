using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
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
        [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
        public virtual Task<PagedResultDto<BlogPostDto>> GetListAsync(BlogPostGetListInput input)
        {
            return BlogPostAdminAppService.GetListAsync(input);
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
