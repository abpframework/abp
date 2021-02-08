using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public interface IMediaDescriptorAdminAppService 
        : ICrudAppService<
            MediaDescriptorDto,
            MediaDescriptorGetListDto,
            Guid, 
            MediaDescriptorGetListInput, 
            UploadMediaStreamContent, 
            UpdateMediaDescriptorDto>
    {
        Task<RemoteStreamContent> DownloadAsync(Guid id, GetMediaRequestDto request);
    }
}