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

namespace Volo.CmsKit.Admin.MediaDescriptors;

[RequiresGlobalFeature(typeof(MediaFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/media")]
public class MediaDescriptorAdminController : CmsKitAdminController, IMediaDescriptorAdminAppService
{
    protected readonly IMediaDescriptorAdminAppService MediaDescriptorAdminAppService;

    public MediaDescriptorAdminController(IMediaDescriptorAdminAppService mediaDescriptorAdminAppService)
    {
        MediaDescriptorAdminAppService = mediaDescriptorAdminAppService;
    }

    [HttpPost]
    [Route("{entityType}")]
    public virtual Task<MediaDescriptorDto> CreateAsync(string entityType, CreateMediaInputWithStream inputStream)
    {
        return MediaDescriptorAdminAppService.CreateAsync(entityType, inputStream);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return MediaDescriptorAdminAppService.DeleteAsync(id);
    }
}
