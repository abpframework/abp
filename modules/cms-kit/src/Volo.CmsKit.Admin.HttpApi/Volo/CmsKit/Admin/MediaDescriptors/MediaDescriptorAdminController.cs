using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Authorize(CmsKitAdminPermissions.MediaDescriptors.Default)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/media")]
    public class MediaDescriptorAdminController : CmsKitAdminController, IMediaDescriptorAdminAppService
    {
        protected readonly IMediaDescriptorAdminAppService MediaDescriptorAdminAppService;

        public MediaDescriptorAdminController(IMediaDescriptorAdminAppService mediaDescriptorAdminAppService)
        {
            MediaDescriptorAdminAppService = mediaDescriptorAdminAppService;
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Create)]
        [NonAction]
        public virtual Task<MediaDescriptorDto> CreateAsync(CreateMediaInputStream inputStream)
        {
            return MediaDescriptorAdminAppService.CreateAsync(inputStream);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return MediaDescriptorAdminAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Create)]
        public virtual async Task<IActionResult> UploadAsync(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            var inputStream = new CreateMediaInputStream(file.OpenReadStream())
                              {
                                  ContentType = file.ContentType,
                                  Name = file.FileName
                              };
            
            var mediaDescriptorDto = await MediaDescriptorAdminAppService.CreateAsync(inputStream);
            
            return StatusCode(201, mediaDescriptorDto);
        }
    }
}