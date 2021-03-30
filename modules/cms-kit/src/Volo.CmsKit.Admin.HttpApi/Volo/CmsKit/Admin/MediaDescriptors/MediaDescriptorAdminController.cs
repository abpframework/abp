using System;
using System.Net;
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
    [RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
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
        [NonAction]
        public virtual Task<MediaDescriptorDto> CreateAsync(CreateMediaInputStream inputStream)
        {
            return MediaDescriptorAdminAppService.CreateAsync(inputStream);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return MediaDescriptorAdminAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("{entityType}")]
        public virtual async Task<IActionResult> UploadAsync(string entityType, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            var inputStream = new CreateMediaInputStream(file.OpenReadStream())
                              {
                                  EntityType = entityType,
                                  ContentType = file.ContentType,
                                  Name = file.FileName
                              };
            
            var mediaDescriptorDto = await MediaDescriptorAdminAppService.CreateAsync(inputStream);
            
            return StatusCode((int)HttpStatusCode.Created, mediaDescriptorDto);
        }
    }
}