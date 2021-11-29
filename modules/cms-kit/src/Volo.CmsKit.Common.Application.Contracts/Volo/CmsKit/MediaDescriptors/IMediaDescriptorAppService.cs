using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.CmsKit.MediaDescriptors
{
    public interface IMediaDescriptorAppService : IApplicationService
    {
        Task<RemoteStreamContent> DownloadAsync(Guid id);
    }
}