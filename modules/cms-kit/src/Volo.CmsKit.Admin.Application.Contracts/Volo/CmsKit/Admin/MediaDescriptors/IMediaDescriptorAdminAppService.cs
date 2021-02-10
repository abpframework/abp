using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public interface IMediaDescriptorAdminAppService : IApplicationService
    {
        Task<MediaDescriptorDto> CreateAsync(CreateMediaInputStream inputStream);
        
        Task<RemoteStreamContent> DownloadAsync(Guid id, GetMediaRequestDto request);

        Task DeleteAsync(Guid id);
    }
}