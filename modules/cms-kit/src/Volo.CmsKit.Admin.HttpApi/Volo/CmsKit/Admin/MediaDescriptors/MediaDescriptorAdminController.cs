using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediasFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
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

        [HttpGet]
        [Route("{id}")]
        public virtual Task<RemoteStreamContent> DownloadAsync(Guid id, GetMediaRequestDto request)
        {
            return MediaDescriptorAdminAppService.DownloadAsync(id, request);
        }

        [HttpPost]
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