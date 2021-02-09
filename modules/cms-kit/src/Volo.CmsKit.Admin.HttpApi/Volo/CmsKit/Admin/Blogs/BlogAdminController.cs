using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Authorize(CmsKitAdminPermissions.Blogs.Default)]
    [Route("api/cms-kit-admin/blogs/blogs")]
    public class BlogAdminController : CmsKitAdminController, IBlogAdminAppService
    {
        protected IBlogAdminAppService BlogAdminAppService { get; }

        public BlogAdminController(IBlogAdminAppService blogAdminAppService)
        {
            BlogAdminAppService = blogAdminAppService;
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Blogs.Create)]
        public Task<BlogDto> CreateAsync(BlogDto input)
        {
            return BlogAdminAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return BlogAdminAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Blogs.Default)]
        public Task<BlogDto> GetAsync(Guid id)
        {
            return BlogAdminAppService.GetAsync(id);
        }

        [HttpGet]
        [Authorize(CmsKitAdminPermissions.Blogs.Default)]
        public Task<PagedResultDto<BlogDto>> GetListAsync([FromQuery] BlogGetListInput input)
        {
            return BlogAdminAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Blogs.Update)]
        public Task<BlogDto> UpdateAsync(Guid id, BlogDto input)
        {
            return BlogAdminAppService.UpdateAsync(id, input);
        }
    }
}
