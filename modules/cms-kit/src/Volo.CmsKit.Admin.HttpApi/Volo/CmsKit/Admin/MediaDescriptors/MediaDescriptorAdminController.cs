using System;
using System.Threading.Tasks;
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
    [Route("api/cms-kit-admin/medias")]
    public class MediaDescriptorAdminController : CmsKitAdminController, IMediaDescriptorAdminAppService
    {
        protected readonly IMediaDescriptorAdminAppService MediaDescriptorAdminAppService;

        public MediaDescriptorAdminController(IMediaDescriptorAdminAppService mediaDescriptorAdminAppService)
        {
            MediaDescriptorAdminAppService = mediaDescriptorAdminAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<MediaDescriptorDto> GetAsync(Guid id)
        {
            return MediaDescriptorAdminAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<MediaDescriptorGetListDto>> GetListAsync(MediaDescriptorGetListInput input)
        {
            return MediaDescriptorAdminAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<MediaDescriptorDto> CreateAsync(UploadMediaStreamContent input)
        {
            return MediaDescriptorAdminAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<MediaDescriptorDto> UpdateAsync(Guid id, UpdateMediaDescriptorDto input)
        {
            return MediaDescriptorAdminAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync(Guid id)
        {
            return MediaDescriptorAdminAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("download/{id}")]
        public virtual Task<RemoteStreamContent> DownloadAsync(Guid id, GetMediaRequestDto request)
        {
            return MediaDescriptorAdminAppService.DownloadAsync(id, request);
        }
    }
}