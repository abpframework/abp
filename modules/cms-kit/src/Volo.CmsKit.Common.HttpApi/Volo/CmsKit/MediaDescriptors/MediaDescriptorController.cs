using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsKitCommonRemoteServiceConsts.ModuleName)]
    [Route("api/cms-kit/media")]
    public class MediaDescriptorController : CmsKitControllerBase, IMediaDescriptorAppService
    {
        protected readonly IMediaDescriptorAppService MediaDescriptorAppService;

        public MediaDescriptorController(IMediaDescriptorAppService mediaDescriptorAppService)
        {
            MediaDescriptorAppService = mediaDescriptorAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<RemoteStreamContent> DownloadAsync(Guid id)
        {
            return MediaDescriptorAppService.DownloadAsync(id);
        }
    }
}
