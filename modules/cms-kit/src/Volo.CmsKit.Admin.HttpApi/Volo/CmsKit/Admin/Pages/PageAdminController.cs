using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Pages
{
    [RequiresGlobalFeature(typeof(PagesFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Authorize(CmsKitAdminPermissions.Pages.Default)]
    [Route("api/cms-kit-admin/pages")]
    public class PageAdminController : CmsKitAdminController, IPageAdminAppService
    {
        protected readonly IPageAdminAppService PageAdminAppService;

        public PageAdminController(IPageAdminAppService pageAdminAppService)
        {
            PageAdminAppService = pageAdminAppService;
        }
        
        [HttpGet]
        [Route("{id}")]
        public virtual Task<PageDto> GetAsync(Guid id)
        {
            return PageAdminAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PageDto>> GetListAsync(GetPagesInputDto input)
        {
            return PageAdminAppService.GetListAsync(input);
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Pages.Create)]
        public virtual Task<PageDto> CreateAsync(CreatePageInputDto input)
        {
            return PageAdminAppService.CreateAsync(input);
        }

        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        [Route("{id}")]
        public virtual Task<PageDto> UpdateAsync(Guid id, UpdatePageInputDto input)
        {
            return PageAdminAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Authorize(CmsKitAdminPermissions.Pages.Delete)]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return PageAdminAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        [Route("image/{id}")]
        public virtual Task SetImageAsync(Guid id, RemoteStreamContent content)
        {
            return PageAdminAppService.SetImageAsync(id, content);
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        [Route("upload-image/{id}")]
        public virtual async Task<IActionResult> UploadImageAsync(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }
            
            await PageAdminAppService.SetImageAsync(id, new RemoteStreamContent(file.OpenReadStream()));
            
            return StatusCode(201);
        }

        [HttpGet]
        [Route("image/{id}")]
        public virtual Task<RemoteStreamContent> GetImageAsync(Guid id)
        {
            Response.Headers.Add("Content-Disposition", "inline;");
            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.ContentType = "image/xyz";
            
            return PageAdminAppService.GetImageAsync(id);
        }
    }
}