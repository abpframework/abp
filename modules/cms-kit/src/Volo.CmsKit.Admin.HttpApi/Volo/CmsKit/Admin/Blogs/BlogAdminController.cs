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
    [Authorize(CmsKitAdminPermissions.Blogs.Default)]
    [Route("api/cms-kit-admin/blogs")]
    public class BlogAdminController : CmsKitAdminController, IBlogAdminAppService
    {
        protected IBlogAdminAppService BlogAdminAppService { get; }

        public BlogAdminController(IBlogAdminAppService blogAdminAppService)
        {
            BlogAdminAppService = blogAdminAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<BlogDto> GetAsync(Guid id)
        {
            return BlogAdminAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<BlogDto>> GetListAsync(BlogGetListInput input)
        {
            return BlogAdminAppService.GetListAsync(input);
        }
        
        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Blogs.Create)]
        public Task<BlogDto> CreateAsync(CreateBlogDto input)
        {
            return BlogAdminAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Blogs.Update)]
        public Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
        {
            return BlogAdminAppService.UpdateAsync(id, input);
        }
        
        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return BlogAdminAppService.DeleteAsync(id);
        }
    }
}
