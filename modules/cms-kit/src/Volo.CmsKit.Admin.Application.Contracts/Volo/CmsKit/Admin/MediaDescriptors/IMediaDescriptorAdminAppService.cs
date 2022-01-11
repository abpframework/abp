using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.MediaDescriptors;

public interface IMediaDescriptorAdminAppService : IApplicationService
{
    Task<MediaDescriptorDto> CreateAsync(string entityType, CreateMediaInputWithStream inputStream);

    Task DeleteAsync(Guid id);
}
